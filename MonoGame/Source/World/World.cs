using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.Player;
using MonoGame_Common.Messages.World;
using MonoGame_Common.States;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame.Source.GameModes;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;
using MonoGame.Source.Systems.Entity.Player;
using MonoGame.Source.Rendering.Utils;

namespace MonoGame.Source.WorldNamespace;

public class World
{
    public Dictionary<GameMode, GameModeController> GameModeControllers { get; set; } = new();
    private Dictionary<System.Drawing.Rectangle, bool[,]> CollisionMasks { get; } = [];

    public World()
    {
        ClientNetworkEventManager.Subscribe<ChunkDataNetworkMessage>(message =>
        {
            LoadChunkFromChunkState(message.ChunkState);
        });

        ClientNetworkEventManager.Subscribe<DeleteTileNetworkMessage>(message =>
        {
            DeleteTile(TileDrawLayer.Tiles, message.PosX, message.PosY);
        });

        ClientNetworkEventManager.Subscribe<PlaceTileNetworkMessage>(message =>
        {
            _ = SetTileAtPosition(message.TileId, TileDrawLayer.Tiles, message.PosX, message.PosY);
        });

        ClientNetworkEventManager.Subscribe<SpawnPlayerNetworkMessage>(message =>
        {
            var player = new Player(message.UUID, message.Position);
            Players.Add(player);
        });

        ClientNetworkEventManager.Subscribe<RenderMaskNetworkMessage>(message =>
        {
            CollisionMasks[message.Rectangle] = message.Mask;
        });

        GameModeControllers[GameMode.Survival] = new SurvivalGameModeController();
        GameModeControllers[GameMode.LevelEditor] = new LevelEditorGameModeController();

        foreach (var controller in GameModeControllers.Values)
        {
            controller.Initialize();
        }
    }

    private List<IGameEntity> Entities { get; } = [];
    public List<Player> Players { get; set; } = [];
    private List<IChunk> Chunks { get; } = [];

    public void LoadChunkFromChunkState(ChunkState chunkState)
    {
        IChunk chunk = new Chunk(chunkState);
        Chunks.Add(chunk);
        chunk.UpdateNeighborChunks();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var entity in GetEntities())
        {
            entity.Update(gameTime);
        }

        GameModeController currentController = GameModeControllers[Globals.World.GetLocalPlayer()?.GameMode ?? GameMode.Survival];
        if (currentController != null)
        {
            currentController.Update();
        }
    }

    public void UpdateAllTextureCoordinates()
    {
        foreach (var chunk in Chunks)
        {
            chunk.UpdateTextureCoordinates();
        }
    }

    public List<IGameEntity> GetEntities()
    {
        return Players.Concat(Entities).ToList();
    }

    public IChunk GetChunkAt(int x, int y)
    {
        return Chunks.Find(c => c.X == x && c.Y == y);
    }

    public IChunk CreateOrGetChunk(int x, int y)
    {
        var existingChunk = GetChunkAt(x, y);
        if (existingChunk == null)
        {
            var chunk = new Chunk(this, x, y);
            Chunks.Add(chunk);
            return chunk;
        }

        return existingChunk;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawWorld();
        foreach (var entity in GetEntities())
        {
            entity.Draw(spriteBatch);
        }
    }

    public Player GetPlayerByUUID(string id)
    {
        return Players.Find(p => p.UUID == id);
    }

    public Player GetLocalPlayer()
    {
        return Players.Where(p => p.UUID == Globals.UUID).FirstOrDefault();
    }

    public CommonTile GetTileAtPosition(Vector2 worldPosition)
    {
        var globalX = (int)(worldPosition.X / Chunk.SizeX);
        var globalY = (int)(worldPosition.Y / Chunk.SizeY);
        return GetTileAt(0, globalX, globalY);
    }

    public static CommonTile SetTileAtPosition(string tile, TileDrawLayer layer, int globalX, int globalY)
    {
        var (LocalX, LocalY) = GetLocalPositionFromGlobalPosition(globalX, globalY);
        var (ChunkPositionX, ChunkPositionY) = GetChunkPositionFromGlobalPosition(globalX, globalY);

        var chunk = Globals.World.CreateOrGetChunk(ChunkPositionX, ChunkPositionY);
        return chunk.SetTileAndUpdateNeighbors(tile, layer, LocalX, LocalY);
    }

    public CommonTile GetTileAt(TileDrawLayer layer, int globalX, int globalY)
    {
        var (LocalX, LocalY) = GetLocalPositionFromGlobalPosition(globalX, globalY);
        var chunk = GetChunkFromGlobalPosition(globalX, globalY);

        return chunk?.GetTile(layer, LocalX, LocalY);
    }

    public List<CommonTile> GetAllTilesFromLayerAt(int globalX, int globalY)
    {
        var chunkX = globalX / Chunk.SizeX;
        var chunkY = globalY / Chunk.SizeY;
        var tileX = globalX % Chunk.SizeX;
        var tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk != null)
        {
            List<CommonTile> tiles = [];
            foreach (var layer in chunk.Tiles.Keys)
            {
                var tile = chunk.GetTile(layer, tileX, tileY);
                tiles.Add(tile);
            }

            return tiles;
        }

        return null;
    }

    public static (int LocalX, int LocalY) GetLocalPositionFromGlobalPosition(int globalPositionX, int globalPositionY)
    {
        var localX = globalPositionX % Chunk.SizeX;
        var localY = globalPositionY % Chunk.SizeY;

        return (localX, localY);
    }

    public static (int ChunkPositionX, int ChunkPositionY) GetChunkPositionFromGlobalPosition(
        int globalPositionX,
        int globalPositionY)
    {
        var chunkX = globalPositionX / Chunk.SizeX;
        var chunkY = globalPositionY / Chunk.SizeY;

        return (chunkX, chunkY);
    }

    public IChunk GetChunkFromGlobalPosition(int globalPositionX, int globalPositionY)
    {
        var (ChunkPositionX, ChunkPositionY) = GetChunkPositionFromGlobalPosition(globalPositionX, globalPositionY);
        return GetChunkAt(ChunkPositionX, ChunkPositionY);
    }

    public static CommonTile GetTileFromScreenPosition(TileDrawLayer layer, int screenX, int screenY)
    {
        var worldPosition = new Vector2(screenX, screenY);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.Camera.Transform));

        var chunkSizeInPixelsX = Chunk.SizeX * SharedGlobals.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * SharedGlobals.PixelSizeY;

        var chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        var chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        var localX = (int)(worldPosition.X % chunkSizeInPixelsX) / SharedGlobals.PixelSizeX;
        var localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / SharedGlobals.PixelSizeY;

        var chunk = Globals.World.GetChunkAt(chunkX, chunkY);
        return chunk?.GetTile(layer, localX, localY) ?? null;
    }

    public static bool Intersects(Rectangle rectA, Rectangle rectB)
    {
        return rectA.X < rectB.X + rectB.Width &&
               rectA.X + rectA.Width > rectB.X &&
               rectA.Y < rectB.Y + rectB.Height &&
               rectA.Y + rectA.Height > rectB.Y;
    }

    public static IChunk GetChunkFromScreenPosition(int layer, int screenX, int screenY)
    {
        var (ChunkPositionX, ChunkPositionY) = GetChunkPositionFromScreenPosition(new Vector2(screenX, screenY));
        var chunk = Globals.World.GetChunkAt(ChunkPositionX, ChunkPositionY);
        return chunk;
    }

    public static (int PosX, int PosY) GetGlobalPositionFromScreenPosition(Vector2 screenPositionBeforeTransform)
    {
        // Inverse transform the screen position to world space
        var screenPosition = Vector2.Transform(screenPositionBeforeTransform, Matrix.Invert(Globals.Camera.Transform));

        // Calculate chunk sizes in pixels
        var chunkSizeInPixelsX = Chunk.SizeX * SharedGlobals.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * SharedGlobals.PixelSizeY;

        // Determine the chunk coordinates
        var chunkX = (int)(screenPosition.X / chunkSizeInPixelsX);
        var chunkY = (int)(screenPosition.Y / chunkSizeInPixelsY);

        // Determine the local position within the chunk
        var localX = (int)(screenPosition.X % chunkSizeInPixelsX) / SharedGlobals.PixelSizeX;
        var localY = (int)(screenPosition.Y % chunkSizeInPixelsY) / SharedGlobals.PixelSizeY;

        // Return the global position
        return ((chunkX * Chunk.SizeX) + localX, (chunkY * Chunk.SizeY) + localY);
    }

    public static (int ChunkPositionX, int ChunkPositionY) GetChunkPositionFromScreenPosition(
        Vector2 screenPositionBeforeTransform)
    {
        var screenPosition = Vector2.Transform(screenPositionBeforeTransform, Matrix.Invert(Globals.Camera.Transform));

        var chunkSizeInPixelsX = Chunk.SizeX * SharedGlobals.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * SharedGlobals.PixelSizeY;

        var chunkX = (int)(screenPosition.X / chunkSizeInPixelsX);
        var chunkY = (int)(screenPosition.Y / chunkSizeInPixelsY);

        return (chunkX, chunkY);
    }

    private void DrawWorld()
    {
        foreach (var chunk in Chunks)
        {
            chunk.Draw(Globals.SpriteBatch);
        }

        DrawCollisionMasks(Globals.SpriteBatch);
    }

    private readonly PrimitiveBatch primitiveBatch = new(Globals.GraphicsDevice.GraphicsDevice);
    private void DrawCollisionMasks(SpriteBatch spriteBatch)
    {
        spriteBatch.End();
        primitiveBatch.Begin(PrimitiveType.LineList);
        foreach (var rectangle in CollisionMasks.Keys)
        {
            var Mask = CollisionMasks[rectangle];
            var drawingArea = rectangle;

            for (var x = 0; x < Mask.GetLength(0); x++)
            {
                for (var y = 0; y < Mask.GetLength(1); y++)
                {
                    if (Mask[x, y])
                    {
                        var topLeft = new Vector2(drawingArea.X + x, drawingArea.Y + y);
                        var topRight = new Vector2(topLeft.X + 1, topLeft.Y);
                        var bottomLeft = new Vector2(topLeft.X, topLeft.Y + 1);
                        var bottomRight = new Vector2(topRight.X, bottomLeft.Y);

                        primitiveBatch.AddVertex(topLeft, Color.Red);
                        primitiveBatch.AddVertex(topRight, Color.Red);

                        primitiveBatch.AddVertex(topRight, Color.Red);
                        primitiveBatch.AddVertex(bottomRight, Color.Red);

                        primitiveBatch.AddVertex(bottomRight, Color.Red);
                        primitiveBatch.AddVertex(bottomLeft, Color.Red);

                        primitiveBatch.AddVertex(bottomLeft, Color.Red);
                        primitiveBatch.AddVertex(topLeft, Color.Red);
                    }
                }
            }
        }

        primitiveBatch.End();
        Globals.DefaultSpriteBatchBegin();
    }

    internal void DeleteTile(TileDrawLayer layer, int posX, int posY)
    {
        var chunk = GetChunkFromGlobalPosition(posX, posY);
        var (LocalX, LocalY) = GetLocalPositionFromGlobalPosition(posX, posY);
        chunk.DeleteTile(layer, LocalX, LocalY);
    }

    public GameModeController GetGameModeController(GameMode gameMode)
    {
        return GameModeControllers[gameMode];
    }
}
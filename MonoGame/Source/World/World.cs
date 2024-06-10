using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.Player;
using MonoGame_Common.Messages.World;
using MonoGame_Common.States;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;
using MonoGame.Source.Systems.Entity.Player;
using MonoGame.Source.Systems.Tiles;

namespace MonoGame.Source.WorldNamespace;

public class World
{
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

    public PlacedTile GetTileAtPosition(Vector2 worldPosition)
    {
        var globalX = (int)(worldPosition.X / Chunk.SizeX);
        var globalY = (int)(worldPosition.Y / Chunk.SizeY);
        return GetTileAt(0, globalX, globalY);
    }

    public PlacedTile SetTileAtPosition(string tile, TileDrawLayer layer, int globalX, int globalY)
    {
        var (LocalX, LocalY) = GetLocalPositionFromGlobalPosition(globalX, globalY);
        var (ChunkPositionX, ChunkPositionY) = GetChunkPositionFromGlobalPosition(globalX, globalY);

        var chunk = Globals.World.CreateOrGetChunk(ChunkPositionX, ChunkPositionY);
        return chunk.SetTileAndUpdateNeighbors(tile, layer, LocalX, LocalY);
    }

    public PlacedTile GetTileAt(TileDrawLayer layer, int globalX, int globalY)
    {
        var (LocalX, LocalY) = GetLocalPositionFromGlobalPosition(globalX, globalY);
        var chunk = GetChunkFromGlobalPosition(globalX, globalY);

        return chunk?.GetTile(layer, LocalX, LocalY);
    }

    public List<PlacedTile> GetAllTilesFromLayerAt(int globalX, int globalY)
    {
        var chunkX = globalX / Chunk.SizeX;
        var chunkY = globalY / Chunk.SizeY;
        var tileX = globalX % Chunk.SizeX;
        var tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk != null)
        {
            List<PlacedTile> tiles = [];
            foreach (var layer in chunk.Tiles.Keys)
            {
                var tile = chunk.GetTile(layer, tileX, tileY);
                tiles.Add(tile);
            }

            return tiles;
        }

        return null;
    }

    public (int LocalX, int LocalY) GetLocalPositionFromGlobalPosition(int globalPositionX, int globalPositionY)
    {
        var localX = globalPositionX % Chunk.SizeX;
        var localY = globalPositionY % Chunk.SizeY;

        return (localX, localY);
    }

    public (int ChunkPositionX, int ChunkPositionY) GetChunkPositionFromGlobalPosition(
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

    public PlacedTile GetTileFromScreenPosition(TileDrawLayer layer, int screenX, int screenY)
    {
        var worldPosition = new Vector2(screenX, screenY);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.Camera.Transform));

        var chunkSizeInPixelsX = Chunk.SizeX * Globals.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Globals.PixelSizeY;

        var chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        var chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        var localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Globals.PixelSizeX;
        var localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Globals.PixelSizeY;

        var chunk = Globals.World.GetChunkAt(chunkX, chunkY);
        return chunk?.GetTile(layer, localX, localY) ?? null;
    }

    public bool Intersects(Rectangle rectA, Rectangle rectB)
    {
        return rectA.X < rectB.X + rectB.Width &&
               rectA.X + rectA.Width > rectB.X &&
               rectA.Y < rectB.Y + rectB.Height &&
               rectA.Y + rectA.Height > rectB.Y;
    }

    public IChunk GetChunkFromScreenPosition(int layer, int screenX, int screenY)
    {
        var (ChunkPositionX, ChunkPositionY) = GetChunkPositionFromScreenPosition(new Vector2(screenX, screenY));
        var chunk = Globals.World.GetChunkAt(ChunkPositionX, ChunkPositionY);
        return chunk;
    }

    public (int PosX, int PosY) GetGlobalPositionFromScreenPosition(Vector2 screenPositionBeforeTransform)
    {
        var screenPosition = Vector2.Transform(screenPositionBeforeTransform, Matrix.Invert(Globals.Camera.Transform));

        var chunkSizeInPixelsX = Chunk.SizeX * Globals.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Globals.PixelSizeY;

        var chunkX = (int)(screenPosition.X / chunkSizeInPixelsX);
        var chunkY = (int)(screenPosition.Y / chunkSizeInPixelsY);

        var localX = (int)(screenPosition.X % chunkSizeInPixelsX) / Globals.PixelSizeX;
        var localY = (int)(screenPosition.Y % chunkSizeInPixelsY) / Globals.PixelSizeY;

        return ((chunkX * Chunk.SizeX) + localX, (chunkY * Chunk.SizeY) + localY);
    }

    public (int ChunkPositionX, int ChunkPositionY) GetChunkPositionFromScreenPosition(
        Vector2 screenPositionBeforeTransform)
    {
        var screenPosition = Vector2.Transform(screenPositionBeforeTransform, Matrix.Invert(Globals.Camera.Transform));

        var chunkSizeInPixelsX = Chunk.SizeX * Globals.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Globals.PixelSizeY;

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
    }

    internal void DeleteTile(TileDrawLayer layer, int posX, int posY)
    {
        var chunk = GetChunkFromGlobalPosition(posX, posY);
        var (LocalX, LocalY) = GetLocalPositionFromGlobalPosition(posX, posY);
        chunk.DeleteTile(layer, LocalX, LocalY);
    }
}
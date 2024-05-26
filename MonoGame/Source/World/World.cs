using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame;

/// <summary>
/// Represents the game world, which contains entities, chunks, and tiles.
/// </summary>
public class World
{
    /// <summary>
    /// Represents a game world that contains entities and chunks.
    /// </summary>
    private List<IGameEntity> Entities { get; set; } = new List<IGameEntity>();
    private List<IChunk> Chunks { get; set; } = new List<IChunk>();
    public Player Player;

    /// <summary>
    /// Initializes the game world by creating a player entity, initializing chunks, and teleporting the player.
    /// </summary>
    public void InitWorld()
    {
        Player = new Player(new Vector2(0, 0));
        Entities.Add(Player);

        InitializeChunks();
        Player.Teleport(new Vector2(500, 500));
    }

    /// <summary>
    /// Updates the game world by updating all entities.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public void Update(GameTime gameTime)
    {
        foreach (var entity in Entities)
        {
            entity.Update(gameTime);
        }
    }

    /// <summary>
    /// Gets the chunk at the specified coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the chunk.</param>
    /// <param name="y">The y-coordinate of the chunk.</param>
    /// <returns>The chunk at the specified coordinates, or null if no chunk is found.</returns>
    public IChunk GetChunkAt(int x, int y)
    {
        return Chunks.Find(c => c.X == x && c.Y == y);
    }

    /// <summary>
    /// Creates a new chunk at the specified coordinates if it doesn't exist, or returns the existing chunk.
    /// </summary>
    /// <param name="x">The x-coordinate of the chunk.</param>
    /// <param name="y">The y-coordinate of the chunk.</param>
    /// <returns>The created or existing chunk.</returns>
    public IChunk CreateOrGetChunk(int x, int y)
    {
        IChunk existingChunk = GetChunkAt(x, y);
        if (existingChunk == null)
        {
            var chunk = new Chunk(this, x, y);
            chunk.Generate();
            Chunks.Add(chunk);
            return chunk;
        }
        else
        {
            return existingChunk;
        }
    }

    /// <summary>
    /// Draws the game world by drawing all chunks and entities.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for drawing.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        DrawWorld();
        foreach (var entity in Entities)
        {
            entity.Draw(spriteBatch);
        }
    }

    /// <summary>
    /// Gets the tile at the specified world position.
    /// </summary>
    /// <param name="worldPosition">The world position.</param>
    /// <returns>The tile at the specified world position, or null if no tile is found.</returns>
    public ITile GetTileAtPosition(Vector2 worldPosition)
    {
        int globalX = (int)(worldPosition.X / Chunk.SizeX);
        int globalY = (int)(worldPosition.Y / Chunk.SizeY);
        return GetTileAt(0, globalX, globalY);
    }

    /// <summary>
    /// Sets the tile at the specified global position.
    /// </summary>
    /// <param name="tile">The tile to set.</param>
    /// <param name="layer">The layer of the tile.</param>
    /// <param name="globalX">The global x-coordinate of the tile.</param>
    /// <param name="globalY">The global y-coordinate of the tile.</param>
    /// <returns>The set tile.</returns>
    public ITile SetTileAtPosition(string tile, TileDrawLayer layer, int globalX, int globalY)
    {
        int chunkX = globalX / Chunk.SizeX;
        int chunkY = globalY / Chunk.SizeY;
        int tileX = globalX % Chunk.SizeX;
        int tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk == null)
        {
            var newChunk = new Chunk(this, tileX, tileY);
            newChunk.Generate();
            Chunks.Add(newChunk);
            chunk = newChunk;
        }

        return chunk.SetTile(tile, layer, tileX, tileY);
    }

    /// <summary>
    /// Gets the tile at the specified global position and layer.
    /// </summary>
    /// <param name="layer">The layer of the tile.</param>
    /// <param name="globalX">The global x-coordinate of the tile.</param>
    /// <param name="globalY">The global y-coordinate of the tile.</param>
    /// <returns>The tile at the specified global position and layer, or null if no tile is found.</returns>
    public ITile GetTileAt(TileDrawLayer layer, int globalX, int globalY)
    {
        int chunkX = globalX / Chunk.SizeX;
        int chunkY = globalY / Chunk.SizeY;
        int tileX = globalX % Chunk.SizeX;
        int tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk != null)
        {
            return chunk.GetTile(layer: layer, x: tileX, y: tileY);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Gets all tiles from all layers at the specified global position.
    /// </summary>
    /// <param name="globalX">The global x-coordinate.</param>
    /// <param name="globalY">The global y-coordinate.</param>
    /// <returns>A list of tiles from all layers at the specified global position, or null if no chunk is found.</returns>
    public List<ITile> GetAllTilesFromLayerAt(int globalX, int globalY)
    {
        int chunkX = globalX / Chunk.SizeX;
        int chunkY = globalY / Chunk.SizeY;
        int tileX = globalX % Chunk.SizeX;
        int tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk != null)
        {
            List<ITile> tiles = new List<ITile>();
            foreach (TileDrawLayer layer in chunk.Tiles.Keys)
            {
                ITile tile = chunk.GetTile(layer, tileX, tileY);
                tiles.Add(tile);
            }
            return tiles;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the tile at the specified screen position and layer.
    /// </summary>
    /// <param name="layer">The layer of the tile.</param>
    /// <param name="screenX">The screen x-coordinate.</param>
    /// <param name="screenY">The screen y-coordinate.</param>
    /// <returns>The tile at the specified screen position and layer, or null if no tile is found.</returns>
    public ITile GetTileAtScreenPosition(TileDrawLayer layer, int screenX, int screenY)
    {
        Vector2 worldPosition = new Vector2(screenX, screenY);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.camera.Transform));

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        IChunk chunk = Globals.world.GetChunkAt(chunkX, chunkY);
        return chunk?.GetTile(layer, localX, localY) ?? null;
    }

    /// <summary>
    /// Gets the tiles that intersect with the specified mask and rectangle.
    /// </summary>
    /// <param name="mask">The mask to check for intersection.</param>
    /// <param name="rectangle">The rectangle to check for intersection.</param>
    /// <returns>A list of tiles that intersect with the specified mask and rectangle.</returns>
    public List<ITile> GetTilesIntersecting(bool[,] mask, Rectangle rectangle)
    {
        Dictionary<string, ITile> intersectingTiles = new Dictionary<string, ITile>();

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int startChunkX = rectangle.Left / chunkSizeInPixelsX;
        int startChunkY = rectangle.Top / chunkSizeInPixelsY;
        int endChunkX = rectangle.Right / chunkSizeInPixelsX;
        int endChunkY = rectangle.Bottom / chunkSizeInPixelsY;

        for (int chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        {
            for (int chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
            {
                IChunk chunk = Globals.world.GetChunkAt(chunkX, chunkY);
                if (chunk != null)
                {
                    int startTileX = Math.Max(0, (rectangle.Left - chunkX * chunkSizeInPixelsX) / Tile.PixelSizeX);
                    int startTileY = Math.Max(0, (rectangle.Top - chunkY * chunkSizeInPixelsY) / Tile.PixelSizeY);
                    int endTileX = Math.Min(Chunk.SizeX - 1, (rectangle.Right - chunkX * chunkSizeInPixelsX) / Tile.PixelSizeX);
                    int endTileY = Math.Min(Chunk.SizeY - 1, (rectangle.Bottom - chunkY * chunkSizeInPixelsY) / Tile.PixelSizeY);

                    List<TileDrawLayer> layers = chunk.Tiles.Keys.ToList();
                    layers.Reverse();
                    layers.Remove(TileDrawLayer.Background);

                    foreach (TileDrawLayer layer in layers)
                    {
                        for (int tileX = startTileX; tileX <= endTileX; tileX++)
                        {
                            for (int tileY = startTileY; tileY <= endTileY; tileY++)
                            {
                                ITile tile = chunk.GetTile(layer: layer, x: tileX, y: tileY);
                                if (tile != null && !tile.Walkable)
                                {
                                    Rectangle tileRect = new Rectangle(
                                        chunkX * chunkSizeInPixelsX + tileX * Tile.PixelSizeX,
                                        chunkY * chunkSizeInPixelsY + tileY * Tile.PixelSizeY,
                                        Tile.PixelSizeX,
                                        Tile.PixelSizeY
                                    );

                                    bool[,] tileMask = CollisionMaskHandler.GetMaskForTexture(tile.SpritesheetName, tile.GetSpriteRectangle());
                                    if (CollisionMaskHandler.CheckMaskCollision(tileMask, rectangle, tileMask, tileRect))
                                    {
                                        if (!intersectingTiles.ContainsKey(tile.Id))
                                        {
                                            intersectingTiles.Add(tile.Id, tile);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return intersectingTiles.Values.ToList();
    }

    /// <summary>
    /// Gets the chunk at the specified screen position.
    /// </summary>
    /// <param name="layer">The layer of the chunk.</param>
    /// <param name="screenX">The screen x-coordinate.</param>
    /// <param name="screenY">The screen y-coordinate.</param>
    /// <returns>The chunk at the specified screen position, or null if no chunk is found.</returns>
    public IChunk GetChunkAtScreenPosition(int layer, int screenX, int screenY)
    {
        Vector2 worldPosition = new Vector2(screenX, screenY);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.camera.Transform));

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        IChunk chunk = Globals.world.GetChunkAt(chunkX, chunkY);
        return chunk;
    }

    /// <summary>
    /// Initializes the chunks in the world.
    /// </summary>
    private void InitializeChunks()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                var chunk = new Chunk(this, x, y);
                chunk.Generate();
                Chunks.Add(chunk);
            }
        }

        foreach (IChunk chunk in Chunks)
        {
            chunk.UpdateTextureCoordinates();
        }
    }

    /// <summary>
    /// Draws the world by iterating through each chunk and calling its Draw method.
    /// </summary>
    private void DrawWorld()
    {
        foreach (var chunk in Chunks)
        {
            chunk.Draw(Globals.spriteBatch);
        }
    }
}

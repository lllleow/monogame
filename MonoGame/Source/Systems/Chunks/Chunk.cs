﻿using System;
using System.Collections.Generic;
using DotnetNoise;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Scripts;
using MonoGame.Source.Util.Loaders;

namespace MonoGame.Source.Systems.Chunks;

/// <summary>
/// Represents a chunk in the game world.
/// </summary>
public class Chunk : IChunk
{
    /// <summary>
    /// Gets or sets the dictionary of tiles in the chunk, organized by tile draw layer.
    /// </summary>
    public Dictionary<TileDrawLayer, ITile[,]> Tiles { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate of the chunk.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the chunk.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Gets or sets the size of the chunk in the X direction.
    /// </summary>
    public static int SizeX { get; set; } = 16;

    /// <summary>
    /// Gets or sets the size of the chunk in the Y direction.
    /// </summary>
    public static int SizeY { get; set; } = 16;

    private World World;

    /// <summary>
    /// Initializes a new instance of the <see cref="Chunk"/> class.
    /// </summary>
    /// <param name="world">The world that the chunk belongs to.</param>
    /// <param name="x">The X coordinate of the chunk.</param>
    /// <param name="y">The Y coordinate of the chunk.</param>
    public Chunk(World world, int x, int y)
    {
        X = x;
        Y = y;
        World = world;
        Tiles = new Dictionary<TileDrawLayer, ITile[,]>();
    }

    /// <summary>
    /// Gets the tile at the specified position in the chunk.
    /// </summary>
    /// <param name="layer">The tile draw layer.</param>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    /// <returns>The tile at the specified position, or null if no tile exists at that position.</returns>
    public ITile GetTile(TileDrawLayer layer, int x, int y)
    {
        if (x >= SizeX || y >= SizeY || x < 0 || y < 0) return null;
        return Tiles[layer][x, y];
    }

    /// <summary>
    /// Generates the tiles for the chunk.
    /// </summary>
    public void Generate()
    {
        FastNoise noise = new FastNoise(seed: new Tuple<int, int>(X, Y).GetHashCode());
        foreach (TileDrawLayer layer in TileDrawLayerPriority.GetPriority())
        {
            Tiles[layer] = new ITile[SizeX, SizeY];
        }

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                SetTile("base.grass", TileDrawLayer.Background, x, y);
                noise.Frequency = 0.1f;

                float noiseValue = noise.GetNoise(x, y);
                float normalizedValue = (noiseValue + 1) / 2;

                if (normalizedValue > 0.7f)
                {
                    SetTile("base.grass", TileDrawLayer.Terrain, x, y);
                }
            }
        }
    }

    /// <summary>
    /// Gets the world position of a tile in the chunk.
    /// </summary>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    /// <returns>The world position of the tile.</returns>
    public Vector2 GetWorldPosition(int x, int y)
    {
        int worldX = X * SizeX + x;
        int worldY = Y * SizeY + y;
        return new Vector2(worldX, worldY);
    }

    /// <summary>
    /// Deletes the tile at the specified position in the chunk.
    /// </summary>
    /// <param name="layer">The tile draw layer.</param>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    public void DeleteTile(TileDrawLayer layer, int x, int y)
    {
        if (x > SizeX || y > SizeY || x < 0 || y < 0) return;
        Tiles[layer][x, y] = null;
        UpdateNeighborChunks();
    }

    /// <summary>
    /// Sets the tile at the specified position in the chunk.
    /// </summary>
    /// <param name="id">The ID of the tile.</param>
    /// <param name="layer">The tile draw layer.</param>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    /// <returns>The tile that was set.</returns>
    public ITile SetTile(string id, TileDrawLayer layer, int x, int y)
    {
        if (x > SizeX || y > SizeY || x < 0 || y < 0) return null;

        ITile tile = TileRegistry.GetTile(id);
        Vector2 worldPosition = GetWorldPosition(x, y);

        tile.Initialize((int)worldPosition.X, (int)worldPosition.Y);

        Tiles[layer][x, y] = tile;
        return tile;
    }

    /// <summary>
    /// Sets the tile at the specified position in the chunk and updates the neighboring chunks.
    /// </summary>
    /// <param name="id">The ID of the tile.</param>
    /// <param name="layer">The tile draw layer.</param>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    /// <returns>The tile that was set.</returns>
    public ITile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y)
    {
        ITile tile = SetTile(id, layer, x, y);
        UpdateNeighborChunks();
        return tile;
    }

    /// <summary>
    /// Gets the direction based on the specified X and Y coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <returns>The direction.</returns>
    public Direction GetDirection(int x, int y)
    {
        if (x == 0 && y == 1) return Direction.Up;
        if (x == 0 && y == -1) return Direction.Down;
        if (x == 1 && y == 0) return Direction.Right;
        if (x == 1 && y == 1) return Direction.RightUp;
        if (x == 1 && y == -1) return Direction.RightDown;
        if (x == -1 && y == 0) return Direction.Left;
        if (x == -1 && y == 1) return Direction.LeftUp;
        if (x == -1 && y == -1) return Direction.LeftDown;
        return Direction.Up;
    }

    /// <summary>
    /// Updates the texture coordinates of all tiles in the chunk.
    /// </summary>
    public void UpdateTextureCoordinates()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                foreach (var layer in Tiles)
                {
                    ITile tile = GetTile(layer.Key, x, y);
                    if (tile != null)
                    {
                        tile.UpdateTextureCoordinates(layer.Key);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates the neighboring tiles of the specified tile draw layer in the chunk.
    /// </summary>
    /// <param name="layer">The tile draw layer.</param>
    public void UpdateNeighborTiles(TileDrawLayer layer)
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                ITile tile = GetTile(layer, x, y);
                if (tile != null)
                {
                    for (int X = -1; X <= 1; X++)
                    {
                        for (int Y = -1; Y <= 1; Y++)
                        {
                            int neighborX = x + X;
                            int neighborY = y + Y;

                            if (neighborX > 0 && neighborY > 0)
                            {
                                Vector2 worldPosition = GetWorldPosition(neighborX, neighborY);
                                ITile neighbor = Globals.world.GetTileAt(layer, (int)worldPosition.X, (int)worldPosition.Y);

                                if (neighbor != null)
                                {
                                    neighbor.OnNeighborChanged(tile, layer, GetDirection(X, Y));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    PrimitiveBatch primitiveBatch = new PrimitiveBatch(Globals.graphicsDevice.GraphicsDevice);
    /// <summary>
    /// Draws the tiles in the chunk using the specified sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to draw with.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var layer in Tiles)
        {
            for (int chunkX = 0; chunkX < SizeX; chunkX++)
            {
                for (int chunkY = 0; chunkY < SizeY; chunkY++)
                {
                    var tile = GetTile(layer.Key, chunkX, chunkY);
                    if (tile != null)
                    {
                        int x = X * SizeX * Tile.PixelSizeX + chunkX * tile.SizeX * Tile.PixelSizeX;
                        int y = Y * SizeY * Tile.PixelSizeY + chunkY * tile.SizeY * Tile.PixelSizeY;

                        Vector2 scale = new Vector2(tile.Scale, tile.Scale);
                        Vector2 origin = new Vector2(tile.SizeX * Tile.PixelSizeX / 2, tile.SizeY * Tile.PixelSizeY / 2);
                        Vector2 position = new Vector2(x, y) + origin;

                        Rectangle tileRectangle = new Rectangle(x, y, tile.SizeX * Tile.PixelSizeX, tile.SizeY * Tile.PixelSizeY);

                        Color colorWithOpacity = Color.White * tile.Opacity;

                        spriteBatch.Draw(
                            SpritesheetLoader.GetSpritesheet(tile.SpritesheetName),
                            position,
                            tile.GetSpriteRectangle(),
                            colorWithOpacity,
                            0f,
                            origin,
                            scale,
                            SpriteEffects.None,
                            0f
                        );

                        if (layer.Key != TileDrawLayer.Background)
                        {
                            Globals.spriteBatch.End();
                            primitiveBatch.Begin(PrimitiveType.LineList);

                            Rectangle rectangle = tileRectangle;
                            Vector2 topLeft = rectangle.Location.ToVector2();
                            Vector2 topRight = new Vector2(rectangle.Right, rectangle.Top);
                            Vector2 bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
                            Vector2 bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);

                            primitiveBatch.AddVertex(topLeft, Color.Red);
                            primitiveBatch.AddVertex(topRight, Color.Red);

                            primitiveBatch.AddVertex(topRight, Color.Red);
                            primitiveBatch.AddVertex(bottomRight, Color.Red);

                            primitiveBatch.AddVertex(bottomRight, Color.Red);
                            primitiveBatch.AddVertex(bottomLeft, Color.Red);

                            primitiveBatch.AddVertex(bottomLeft, Color.Red);
                            primitiveBatch.AddVertex(topLeft, Color.Red);

                            primitiveBatch.End();
                            Globals.spriteBatch.Begin(transformMatrix: Globals.camera.Transform, sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Initializes the chunk.
    /// </summary>
    public void Initialize()
    {
    }

    /// <summary>
    /// Updates the neighboring chunks of the chunk.
    /// </summary>
    public void UpdateNeighborChunks()
    {
        UpdateTextureCoordinates();

        int chunkXMinus = X - 1;
        int chunkXPlus = X + 1;
        int chunkYMinus = Y - 1;
        int chunkYPlus = Y + 1;

        IChunk chunkMinusX = Globals.world.GetChunkAt(chunkXMinus, Y);
        IChunk chunkPlusX = Globals.world.GetChunkAt(chunkXPlus, Y);
        IChunk chunkMinusY = Globals.world.GetChunkAt(X, chunkYMinus);
        IChunk chunkPlusY = Globals.world.GetChunkAt(X, chunkYPlus);

        if (chunkMinusX != null)
        {
            chunkMinusX.UpdateTextureCoordinates();
        }

        if (chunkPlusX != null)
        {
            chunkPlusX.UpdateTextureCoordinates();
        }

        if (chunkMinusY != null)
        {
            chunkMinusY.UpdateTextureCoordinates();
        }

        if (chunkPlusY != null)
        {
            chunkPlusY.UpdateTextureCoordinates();
        }
    }
}

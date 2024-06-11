﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Common.Enums;
using MonoGame_Common.States;
using MonoGame_Common.Util.Enum;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Scripts;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Utils.Loaders;
using MonoGame.Source.WorldNamespace;

namespace MonoGame.Source.Systems.Chunks;

public class Chunk : IChunk
{
    private readonly PrimitiveBatch primitiveBatch = new(Globals.GraphicsDevice.GraphicsDevice);

    private readonly World world = Globals.World;

    public Chunk(World world, int x, int y)
    {
        X = x;
        Y = y;
        this.world = world;
        Tiles = [];

        foreach (var layer in TileDrawLayerPriority.GetPriority())
        {
            Tiles[layer] = new Tile[SizeX, SizeY];
        }

        for (var chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (var chunkY = 0; chunkY < SizeY; chunkY++)
            {
                _ = SetTile("base.grass", TileDrawLayer.Background, chunkX, chunkY);
            }
        }
    }

    public Chunk(ChunkState chunkState)
    {
        X = chunkState.X;
        Y = chunkState.Y;
        Tiles = [];

        foreach (var layer in TileDrawLayerPriority.GetPriority())
        {
            Tiles[layer] = new Tile[SizeX, SizeY];
        }

        foreach (var tileState in chunkState.Tiles)
        {
            _ = SetTile(tileState.Id, tileState.Layer, tileState.LocalX.Value, tileState.LocalY.Value);
        }

        for (var chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (var chunkY = 0; chunkY < SizeY; chunkY++)
            {
                _ = SetTile("base.grass", TileDrawLayer.Background, chunkX, chunkY);
            }
        }

        Globals.World.UpdateAllTextureCoordinates();
    }

    public static int SizeX { get; set; } = 16;

    public static int SizeY { get; set; } = 16;
    public Dictionary<TileDrawLayer, Tile[,]> Tiles { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public Tile GetTile(TileDrawLayer layer, int x, int y)
    {
        return x >= SizeX || y >= SizeY || x < 0 || y < 0 ? null : Tiles[layer][x, y];
    }

    public void DeleteTile(TileDrawLayer layer, int x, int y)
    {
        if (x > SizeX || y > SizeY || x < 0 || y < 0)
        {
            return;
        }

        Tiles[layer][x, y] = null;
        UpdateNeighborChunks();
    }

    public Tile SetTile(string id, TileDrawLayer layer, int x, int y)
    {
        if (x > SizeX || y > SizeY || x < 0 || y < 0)
        {
            return null;
        }

        var tile = TileRegistry.GetTile(id);
        var worldPosition = GetWorldPosition(x, y);

        Tiles[layer][x, y] = tile;
        return tile;
    }

    public Tile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y)
    {
        var Tile = SetTile(id, layer, x, y);
        UpdateNeighborChunks();
        return Tile;
    }

    public void UpdateTextureCoordinates()
    {
        for (var x = 0; x < SizeX; x++)
        {
            for (var y = 0; y < SizeY; y++)
            {
                foreach (var layer in Tiles)
                {
                    var tile = GetTile(layer.Key, x, y);
                    if (tile?.HasComponent<TextureRendererTileComponent>() ?? false)
                    {
                        TextureRendererTileComponent textureRendererTileComponent = tile.GetComponent<TextureRendererTileComponent>();
                        (int localX, int localY) = GetTilePosition(tile);
                        var worldPosition = GetWorldPosition(localX, localY);
                        textureRendererTileComponent.UpdateTextureCoordinates((int)worldPosition.X, (int)worldPosition.Y, layer.Key);
                    }
                }
            }
        }
    }

    public (int PosX, int PosY) GetTilePosition(Tile tile)
    {
        for (var x = 0; x < SizeX; x++)
        {
            for (var y = 0; y < SizeY; y++)
            {
                foreach (var layer in Tiles)
                {
                    if (GetTile(layer.Key, x, y) == tile)
                    {
                        return (x, y);
                    }
                }
            }
        }

        return (-1, -1);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var layer in Tiles)
        {
            for (var chunkX = 0; chunkX < SizeX; chunkX++)
            {
                for (var chunkY = 0; chunkY < SizeY; chunkY++)
                {
                    var tile = GetTile(layer.Key, chunkX, chunkY);
                    if (tile != null)
                    {
                        var x = (X * SizeX * Globals.PixelSizeX) + (chunkX * tile.TileSizeX * Globals.PixelSizeX);
                        var y = (Y * SizeY * Globals.PixelSizeY) + (chunkY * tile.TileSizeY * Globals.PixelSizeY);

                        var scale = new Vector2(1, 1);
                        var origin = new Vector2(tile.TileSizeX * Globals.PixelSizeX / 2, tile.TileSizeX * Globals.PixelSizeY / 2);
                        var position = new Vector2(x, y) + origin;

                        var tileRectangle = new Rectangle(x, y, tile.TileSizeX * Globals.PixelSizeX, tile.TileSizeY * Globals.PixelSizeY);
                        var colorWithOpacity = Color.White;

                        var layerDepth = 1f;

                        if (layer.Key == TileDrawLayer.Terrain)
                        {
                            layerDepth = 0.1f;
                        }

                        if (layer.Key == TileDrawLayer.Background)
                        {
                            layerDepth = 0f;
                        }

                        if (tile?.HasComponent<TextureRendererTileComponent>() ?? false)
                        {
                            TextureRendererTileComponent textureRendererTileComponent = tile.GetComponent<TextureRendererTileComponent>();

                            spriteBatch.Draw(
                                SpritesheetLoader.GetSpritesheet(tile.SpritesheetName),
                                position,
                                textureRendererTileComponent.GetSpriteRectangle(),
                                colorWithOpacity,
                                0f,
                                origin,
                                scale,
                                SpriteEffects.None,
                                layerDepth
                            );

                            if (tile.CollisionMaskSpritesheetName != null && tile.CollisionMode == CollisionMode.CollisionMask)
                            {
                                spriteBatch.Draw(
                                    SpritesheetLoader.GetSpritesheet(tile.CollisionMaskSpritesheetName),
                                    position,
                                    textureRendererTileComponent.GetSpriteRectangle(),
                                    colorWithOpacity,
                                    0f,
                                    origin,
                                    scale,
                                    SpriteEffects.None,
                                    1f
                                );
                            }

                            if (layer.Key != TileDrawLayer.Background && Globals.ShowTileBoundingBox)
                            {
                                Globals.SpriteBatch.End();
                                primitiveBatch.Begin(PrimitiveType.LineList);

                                var rectangle = tileRectangle;
                                var topLeft = rectangle.Location.ToVector2();
                                var topRight = new Vector2(rectangle.Right, rectangle.Top);
                                var bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
                                var bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);

                                primitiveBatch.AddVertex(topLeft, Color.Red);
                                primitiveBatch.AddVertex(topRight, Color.Red);

                                primitiveBatch.AddVertex(topRight, Color.Red);
                                primitiveBatch.AddVertex(bottomRight, Color.Red);

                                primitiveBatch.AddVertex(bottomRight, Color.Red);
                                primitiveBatch.AddVertex(bottomLeft, Color.Red);

                                primitiveBatch.AddVertex(bottomLeft, Color.Red);
                                primitiveBatch.AddVertex(topLeft, Color.Red);

                                primitiveBatch.End();

                                Globals.DefaultSpriteBatchBegin();
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateNeighborChunks()
    {
        UpdateTextureCoordinates();

        var chunkXMinus = X - 1;
        var chunkXPlus = X + 1;
        var chunkYMinus = Y - 1;
        var chunkYPlus = Y + 1;

        var chunkMinusX = Globals.World.GetChunkAt(chunkXMinus, Y);
        var chunkPlusX = Globals.World.GetChunkAt(chunkXPlus, Y);
        var chunkMinusY = Globals.World.GetChunkAt(X, chunkYMinus);
        var chunkPlusY = Globals.World.GetChunkAt(X, chunkYPlus);
        var chunkMinusXMinusY = Globals.World.GetChunkAt(chunkXMinus, chunkYMinus);
        var chunkMinusXPlusY = Globals.World.GetChunkAt(chunkXMinus, chunkYPlus);
        var chunkPlusXMinusY = Globals.World.GetChunkAt(chunkXPlus, chunkYMinus);
        var chunkPlusXPlusY = Globals.World.GetChunkAt(chunkXPlus, chunkYPlus);

        chunkMinusX?.UpdateTextureCoordinates();
        chunkPlusX?.UpdateTextureCoordinates();
        chunkMinusY?.UpdateTextureCoordinates();
        chunkPlusY?.UpdateTextureCoordinates();
        chunkMinusXMinusY?.UpdateTextureCoordinates();
        chunkMinusXPlusY?.UpdateTextureCoordinates();
        chunkPlusXMinusY?.UpdateTextureCoordinates();
        chunkPlusXPlusY?.UpdateTextureCoordinates();
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        var worldX = (X * SizeX) + x;
        var worldY = (Y * SizeY) + y;
        return new Vector2(worldX, worldY);
    }

    public void Initialize()
    {
    }
}
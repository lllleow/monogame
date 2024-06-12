using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.States;
using MonoGame_Common.Systems.Scripts;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Tile;
using MonoGame_Common.Util.Tile.TileComponents;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Utils;
using MonoGame.Source.Utils.Helpers;
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
            Tiles[layer] = new CommonTile[SizeX, SizeY];
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
            Tiles[layer] = new CommonTile[SizeX, SizeY];
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
    }

    public static int SizeX { get; set; } = 16;

    public static int SizeY { get; set; } = 16;
    public Dictionary<TileDrawLayer, CommonTile[,]> Tiles { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public CommonTile GetTile(TileDrawLayer layer, int x, int y)
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

    public CommonTile SetTile(string id, TileDrawLayer layer, int x, int y)
    {
        if (x > SizeX || y > SizeY || x < 0 || y < 0)
        {
            return null;
        }

        var tile = (CommonTile)TileRegistry.GetTile(id);
        _ = GetWorldPosition(x, y);

        Tiles[layer][x, y] = tile;
        return tile;
    }

    public CommonTile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y)
    {
        var tile = SetTile(id, layer, x, y);
        UpdateNeighborChunks();
        return tile;
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
                        var worldPosition = GetWorldPosition(x, y);
                        TileNeighborConfiguration tileNeighborConfiguration = TileClientHelper.GetNeighborConfiguration(tile, layer.Key, (int)worldPosition.X, (int)worldPosition.Y);
                        textureRendererTileComponent.UpdateTextureCoordinates(tileNeighborConfiguration, layer.Key);
                    }
                }
            }
        }
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
                        var x = (X * SizeX * SharedGlobals.PixelSizeX) + (chunkX * tile.TileSizeX * SharedGlobals.PixelSizeX);
                        var y = (Y * SizeY * SharedGlobals.PixelSizeY) + (chunkY * tile.TileSizeY * SharedGlobals.PixelSizeY);

                        var scale = new Vector2(1, 1);
                        var origin = new Vector2(tile.TileSizeX * SharedGlobals.PixelSizeX / 2, tile.TileSizeX * SharedGlobals.PixelSizeY / 2);
                        var position = new Vector2(x, y) + origin;

                        var tileRectangle = new Rectangle(x, y, tile.TileSizeX * SharedGlobals.PixelSizeX, tile.TileSizeY * SharedGlobals.PixelSizeY);
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
                                RectangleHelper.ConvertToXNARectangle(textureRendererTileComponent.GetSpriteRectangle()),
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
                                    RectangleHelper.ConvertToXNARectangle(textureRendererTileComponent.GetSpriteRectangle()),
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
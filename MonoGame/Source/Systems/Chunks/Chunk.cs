using System.Collections.Generic;
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

        foreach (var layer in TileDrawLayerPriority.GetPriority()) Tiles[layer] = new ITile[SizeX, SizeY];

        for (var chunkX = 0; chunkX < SizeX; chunkX++)
        for (var chunkY = 0; chunkY < SizeY; chunkY++)
            _ = SetTile("base.grass", TileDrawLayer.Background, chunkX, chunkY);
    }

    public Chunk(ChunkState chunkState)
    {
        X = chunkState.X;
        Y = chunkState.Y;
        Tiles = [];

        foreach (var layer in TileDrawLayerPriority.GetPriority()) Tiles[layer] = new ITile[SizeX, SizeY];

        foreach (var tileState in chunkState.Tiles)
            _ = SetTile(tileState.Id, tileState.Layer, tileState.LocalX.Value, tileState.LocalY.Value);

        for (var chunkX = 0; chunkX < SizeX; chunkX++)
        for (var chunkY = 0; chunkY < SizeY; chunkY++)
            _ = SetTile("base.grass", TileDrawLayer.Background, chunkX, chunkY);

        Globals.World.UpdateAllTextureCoordinates();
    }

    public static int SizeX { get; set; } = 16;

    public static int SizeY { get; set; } = 16;
    public Dictionary<TileDrawLayer, ITile[,]> Tiles { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public ITile GetTile(TileDrawLayer layer, int x, int y)
    {
        return x >= SizeX || y >= SizeY || x < 0 || y < 0 ? null : Tiles[layer][x, y];
    }

    public void DeleteTile(TileDrawLayer layer, int x, int y)
    {
        if (x > SizeX || y > SizeY || x < 0 || y < 0) return;

        Tiles[layer][x, y] = null;
        UpdateNeighborChunks();
    }

    public ITile SetTile(string id, TileDrawLayer layer, int x, int y)
    {
        if (x > SizeX || y > SizeY || x < 0 || y < 0) return null;

        var tile = TileRegistry.GetTile(id);
        var worldPosition = GetWorldPosition(x, y);

        tile.Initialize(x, y, (int)worldPosition.X, (int)worldPosition.Y);

        Tiles[layer][x, y] = tile;
        return tile;
    }

    public ITile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y)
    {
        var tile = SetTile(id, layer, x, y);
        UpdateNeighborChunks();
        return tile;
    }

    public void UpdateTextureCoordinates()
    {
        for (var x = 0; x < SizeX; x++)
        for (var y = 0; y < SizeY; y++)
            foreach (var layer in Tiles)
            {
                var tile = GetTile(layer.Key, x, y);
                tile?.UpdateTextureCoordinates(layer.Key);
            }
    }

    public void UpdateNeighborTiles(TileDrawLayer layer)
    {
        for (var x = 0; x < SizeX; x++)
        for (var y = 0; y < SizeY; y++)
        {
            var tile = GetTile(layer, x, y);
            if (tile != null)
                for (var X = -1; X <= 1; X++)
                for (var Y = -1; Y <= 1; Y++)
                {
                    var neighborX = x + X;
                    var neighborY = y + Y;

                    if (neighborX > 0 && neighborY > 0)
                    {
                        var worldPosition = GetWorldPosition(neighborX, neighborY);
                        var neighbor = Globals.World.GetTileAt(layer, (int)worldPosition.X, (int)worldPosition.Y);

                        neighbor?.OnNeighborChanged(tile, layer, DirectionHelper.GetDirection(X, Y));
                    }
                }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var layer in Tiles)
            for (var chunkX = 0; chunkX < SizeX; chunkX++)
            for (var chunkY = 0; chunkY < SizeY; chunkY++)
            {
                var tile = GetTile(layer.Key, chunkX, chunkY);
                if (tile != null)
                {
                    var x = X * SizeX * Tile.PixelSizeX + chunkX * tile.SizeX * Tile.PixelSizeX;
                    var y = Y * SizeY * Tile.PixelSizeY + chunkY * tile.SizeY * Tile.PixelSizeY;

                    var scale = new Vector2(tile.Scale, tile.Scale);
                    var origin = new Vector2(tile.SizeX * Tile.PixelSizeX / 2, tile.SizeY * Tile.PixelSizeY / 2) +
                                 new Vector2(tile.PixelOffsetX, tile.PixelOffsetY);
                    var position = new Vector2(x, y) + origin;

                    var tileRectangle = new Rectangle(x, y, tile.SizeX * Tile.PixelSizeX, tile.SizeY * Tile.PixelSizeY);
                    var colorWithOpacity = Color.White * tile.Opacity;

                    var layerDepth = 1f;

                    if (layer.Key == TileDrawLayer.Terrain) layerDepth = 0.1f;

                    if (layer.Key == TileDrawLayer.Background) layerDepth = 0f;

                    spriteBatch.Draw(
                        SpritesheetLoader.GetSpritesheet(tile.SpritesheetName),
                        position,
                        tile.GetSpriteRectangle(),
                        colorWithOpacity,
                        0f,
                        origin,
                        scale,
                        SpriteEffects.None,
                        layerDepth
                    );

                    if (tile.CollisionMaskSpritesheetName != null && tile.CollisionMode == CollisionMode.CollisionMask)
                        spriteBatch.Draw(
                            SpritesheetLoader.GetSpritesheet(tile.CollisionMaskSpritesheetName),
                            position,
                            tile.GetSpriteRectangle(),
                            colorWithOpacity,
                            0f,
                            origin,
                            scale,
                            SpriteEffects.None,
                            1f
                        );

                    if (layer.Key != TileDrawLayer.Background && Tile.ShowTileBoundingBox)
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
        var worldX = X * SizeX + x;
        var worldY = Y * SizeY + y;
        return new Vector2(worldX, worldY);
    }

    public void Initialize()
    {
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Scripts;
using MonoGame.Source.Util.Loaders;

namespace MonoGame.Source.Systems.Chunks;

public class Chunk : IChunk
{

    public Dictionary<TileDrawLayer, ITile[,]> Tiles { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public static int SizeX { get; set; } = 16;

    public static int SizeY { get; set; } = 16;

    private World World = Globals.World;

    public Chunk(World world, int x, int y)
    {
        X = x;
        Y = y;
        World = world;
        Tiles = new Dictionary<TileDrawLayer, ITile[,]>();

        foreach (TileDrawLayer layer in TileDrawLayerPriority.GetPriority())
        {
            Tiles[layer] = new ITile[SizeX, SizeY];
        }

        for (int chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (int chunkY = 0; chunkY < SizeY; chunkY++)
            {
                SetTile("base.grass", TileDrawLayer.Background, chunkX, chunkY);
            }
        }
    }

    public Chunk(ChunkState chunkState)
    {
        X = chunkState.X;
        Y = chunkState.Y;
        Tiles = new Dictionary<TileDrawLayer, ITile[,]>();

        foreach (TileDrawLayer layer in TileDrawLayerPriority.GetPriority())
        {
            Tiles[layer] = new ITile[SizeX, SizeY];
        }

        foreach (TileState tileState in chunkState.Tiles)
        {
            SetTile(tileState.Id, tileState.Layer.Value, tileState.LocalX.Value, tileState.LocalY.Value);
        }

        for (int chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (int chunkY = 0; chunkY < SizeY; chunkY++)
            {
                SetTile("base.grass", TileDrawLayer.Background, chunkX, chunkY);
            }
        }

        Globals.World.UpdateAllTextureCoordinates();
    }

    public ITile GetTile(TileDrawLayer layer, int x, int y)
    {
        if (x >= SizeX || y >= SizeY || x < 0 || y < 0) return null;
        return Tiles[layer][x, y];
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        int worldX = X * SizeX + x;
        int worldY = Y * SizeY + y;
        return new Vector2(worldX, worldY);
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

        ITile tile = TileRegistry.GetTile(id);
        Vector2 worldPosition = GetWorldPosition(x, y);

        tile.Initialize(x, y, (int)worldPosition.X, (int)worldPosition.Y);

        Tiles[layer][x, y] = tile;
        return tile;
    }

    public ITile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y)
    {
        ITile tile = SetTile(id, layer, x, y);
        UpdateNeighborChunks();
        return tile;
    }

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
                                ITile neighbor = Globals.World.GetTileAt(layer, (int)worldPosition.X, (int)worldPosition.Y);

                                if (neighbor != null)
                                {
                                    neighbor.OnNeighborChanged(tile, layer, DirectionHelper.GetDirection(X, Y));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    PrimitiveBatch primitiveBatch = new PrimitiveBatch(Globals.GraphicsDevice.GraphicsDevice);

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
                        Vector2 origin = new Vector2(tile.SizeX * Tile.PixelSizeX / 2, tile.SizeY * Tile.PixelSizeY / 2) + new Vector2(tile.PixelOffsetX, tile.PixelOffsetY);
                        Vector2 position = new Vector2(x, y) + origin;

                        Rectangle tileRectangle = new Rectangle(x, y, tile.SizeX * Tile.PixelSizeX, tile.SizeY * Tile.PixelSizeY);
                        Color colorWithOpacity = Color.White * tile.Opacity;

                        float layerDepth = 1f;

                        if (layer.Key == TileDrawLayer.Terrain)
                        {
                            layerDepth = 0.1f;
                        }

                        if (layer.Key == TileDrawLayer.Background)
                        {
                            layerDepth = 0f;
                        }

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

                        if (tile.CollisionMaskSpritesheetName != null && tile.CollisionMode == CollisionMode.CollisionMask && CollisionComponent.ShowCollisionMasks)
                        {
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
                        }

                        if (layer.Key != TileDrawLayer.Background && Tile.ShowTileBoundingBox)
                        {
                            Globals.SpriteBatch.End();
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

                            Globals.DefaultSpriteBatchBegin();
                        }
                    }
                }
            }
        }
    }

    public void Initialize()
    {
    }

    public void UpdateNeighborChunks()
    {
        UpdateTextureCoordinates();

        int chunkXMinus = X - 1;
        int chunkXPlus = X + 1;
        int chunkYMinus = Y - 1;
        int chunkYPlus = Y + 1;

        IChunk chunkMinusX = Globals.World.GetChunkAt(chunkXMinus, Y);
        IChunk chunkPlusX = Globals.World.GetChunkAt(chunkXPlus, Y);
        IChunk chunkMinusY = Globals.World.GetChunkAt(X, chunkYMinus);
        IChunk chunkPlusY = Globals.World.GetChunkAt(X, chunkYPlus);
        IChunk chunkMinusXMinusY = Globals.World.GetChunkAt(chunkXMinus, chunkYMinus);
        IChunk chunkMinusXPlusY = Globals.World.GetChunkAt(chunkXMinus, chunkYPlus);
        IChunk chunkPlusXMinusY = Globals.World.GetChunkAt(chunkXPlus, chunkYMinus);
        IChunk chunkPlusXPlusY = Globals.World.GetChunkAt(chunkXPlus, chunkYPlus);

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

        if (chunkMinusXMinusY != null)
        {
            chunkMinusXMinusY.UpdateTextureCoordinates();
        }

        if (chunkMinusXPlusY != null)
        {
            chunkMinusXPlusY.UpdateTextureCoordinates();
        }

        if (chunkPlusXMinusY != null)
        {
            chunkPlusXMinusY.UpdateTextureCoordinates();
        }

        if (chunkPlusXPlusY != null)
        {
            chunkPlusXPlusY.UpdateTextureCoordinates();
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class Chunk : IChunk
{
    public Dictionary<TileDrawLayer, ITile[,]> Tiles { get; set; }
    public BiomeGenerationConditions[,] Biome { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public static int SizeX { get; set; } = 16;
    public static int SizeY { get; set; } = 16;
    private World World;
    private TemperatureSampler temperatureSampler = new TemperatureSampler();
    private ElevationSampler elevationSampler = new ElevationSampler();
    private UrbanizationSampler urbanizationSampler = new UrbanizationSampler();
    private RadiationSampler radiationSampler = new RadiationSampler();

    public Chunk(World world, int x, int y)
    {
        X = x;
        Y = y;
        World = world;
        Tiles = new Dictionary<TileDrawLayer, ITile[,]>();
    }

    public ITile GetTile(TileDrawLayer layer, int x, int y)
    {
        if (x >= SizeX || y >= SizeY || x < 0 || y < 0) return null;
        return Tiles[layer][x, y];
    }

    public void Generate()
    {
        foreach (TileDrawLayer layer in TileDrawLayerPriority.GetPriority())
        {
            Tiles[layer] = new ITile[SizeX, SizeY];
        }

        Biome = new BiomeGenerationConditions[SizeX, SizeY];
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                SetTile("base.water", TileDrawLayer.Background, x, y);
                SetTile("base.grass", TileDrawLayer.Terrain, x, y);
            }
        }
    }

    public IBiome GetCurrentBiome(BiomeGenerationConditions conditions)
    {
        foreach (var biomeEntry in BiomeRegistry.Biomes)
        {
            IBiome biomeInstance = BiomeRegistry.GetBiome(biomeEntry.Key);
            if (biomeInstance != null && biomeInstance.Enabled && conditions.ElevationThreshold < biomeInstance.BiomeGenerationConditions.ElevationThreshold)
            {
                return biomeInstance;
            }
        }

        return null;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        int worldX = (X * SizeX) + x;
        int worldY = (Y * SizeY) + y;
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
        tile.Initialize((int)worldPosition.X, (int)worldPosition.Y);
        Tiles[layer][x, y] = tile;
        return tile;
    }

    public ITile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y)
    {
        ITile tile = SetTile(id, layer, x, y);
        UpdateNeighborChunks();
        return tile;
    }

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
                        int x = (X * SizeX * Tile.PixelSizeX) + (chunkX * tile.SizeX * Tile.PixelSizeX);
                        int y = (Y * SizeY * Tile.PixelSizeY) + (chunkY * tile.SizeY * Tile.PixelSizeY);
                        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(tile.SpritesheetName), new Vector2(x, y), tile.GetSpriteRectangle(), Color.White);
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

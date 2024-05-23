using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class Chunk : IChunk
{
    public ITile[,] Tiles { get; set; }
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

    public Chunk(World world, int X, int Y)
    {
        this.X = X;
        this.Y = Y;
        this.World = world;
    }

    public ITile GetTile(int x, int y)
    {
        if (x >= SizeX || y >= SizeY || x < 0 || y < 0) return null;
        return Tiles[x, y];
    }
    public void Generate()
    {
        Tiles = new ITile[SizeX, SizeY];

        Biome = new BiomeGenerationConditions[SizeX, SizeY];
        for (int chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (int chunkY = 0; chunkY < SizeY; chunkY++)
            {
                Vector2 worldPosition = GetWorldPosition(chunkX, chunkY);

                double temperature = temperatureSampler.Sample(worldPosition.X, worldPosition.Y);
                double elevation = elevationSampler.Sample(worldPosition.X, worldPosition.Y);
                double urbanization = urbanizationSampler.Sample(worldPosition.X, worldPosition.Y);
                double radiation = radiationSampler.Sample(worldPosition.X, worldPosition.Y);

                BiomeGenerationConditions conditions = new BiomeGenerationConditions(temperature, elevation, urbanization, radiation);
                Biome[chunkX, chunkY] = conditions;

                SetTile(GetCurrentBiome(conditions).SampleBiomeTile(chunkX, chunkY), chunkX, chunkY);
            }
        }
    }

    public IBiome GetCurrentBiome(BiomeGenerationConditions conditions)
    {
        foreach (var biomeEntry in BiomeRegistry.Biomes)
        {
            IBiome biomeInstance = BiomeRegistry.GetBiome(biomeEntry.Key);
            if (biomeInstance != null && conditions.ElevationThreshold < biomeInstance.BiomeGenerationConditions.ElevationThreshold)
            {
                return biomeInstance;
            }
        }
        return BiomeRegistry.GetBiome("base.biome.ocean");
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        int worldX = (X * SizeX) + x;
        int worldY = (Y * SizeY) + y;
        return new Vector2(worldX, worldY);
    }

    public ITile SetTile(string id, int x, int y)
    {
        ITile tile = TileRegistry.GetTile(id);
        Vector2 worldPosition = GetWorldPosition(x, y);
        tile.Initialize((int) worldPosition.X, (int) worldPosition.Y);
        Tiles[x, y] = tile;
        return tile;
    }

    public Direction GetDirection(int x, int y)
    {
        if (x == 0 && y == 1) return Direction.TOP;
        if (x == 0 && y == -1) return Direction.BOTTOM;
        if (x == 1 && y == 0) return Direction.RIGHT;
        if (x == 1 && y == 1) return Direction.RIGHT_TOP;
        if (x == 1 && y == -1) return Direction.RIGHT_BOTTOM;
        if (x == -1 && y == 0) return Direction.LEFT;
        if (x == -1 && y == 1) return Direction.LEFT_TOP;
        if (x == -1 && y == -1) return Direction.LEFT_BOTTOM;
        return Direction.TOP;
    }

    public void UpdateTextureCoordinates()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                ITile tile = GetTile(x, y);
                if (tile != null)
                {
                    tile.UpdateTextureCoordinates();
                }
            }
        }
    }
    public void UpdateNeighborTiles()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                ITile tile = GetTile(x, y);
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
                                ITile neighbor = Globals.world.GetTileAt((int)worldPosition.X, (int)worldPosition.Y);

                                if (neighbor != null)
                                {
                                    neighbor.OnNeighborChanged(tile, GetDirection(X, Y));
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
        for (int chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (int chunkY = 0; chunkY < SizeY; chunkY++)
            {
                var tile = GetTile(chunkX, chunkY);
                if (tile != null)
                {
                    int x = (X * SizeX * Tile.PixelSizeX) + (chunkX * tile.SizeX * Tile.PixelSizeX);
                    int y = (Y * SizeY * Tile.PixelSizeY) + (chunkY * tile.SizeY * Tile.PixelSizeY);
                    spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(tile.SpritesheetName), new Vector2(x, y), tile.GetSpriteRectangle(), Color.White);
                }
            }
        }
    }

    public void Initialize()
    {
    }
}

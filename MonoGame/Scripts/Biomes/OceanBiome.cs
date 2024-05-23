using System;

public class OceanBiome : IBiome
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public BiomeGenerationConditions BiomeGenerationConditions { get; set; }

    public OceanBiome()
    {
        Id = "base.biome.ocean";
        Name = "Ocean Biome";
        Enabled = false;
        BiomeGenerationConditions = new BiomeGenerationConditions(temperatureThreshold: 1, elevationThreshold: 1, urbanizationThreshold: 1, radiationThreshold: 1);
    }

    public string SampleBiomeTile(int x, int y)
    {
        return "base.water";
    }
}

return new OceanBiome();

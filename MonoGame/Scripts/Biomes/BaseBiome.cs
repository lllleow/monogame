using System;

public class BaseBiome : IBiome
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public BiomeGenerationConditions BiomeGenerationConditions { get; set; }

    public BaseBiome()
    {
        Id = "base.biome";
        Name = "Base Biome";
        Enabled = true;
        BiomeGenerationConditions = new BiomeGenerationConditions(temperatureThreshold: 1, elevationThreshold: 0.5, urbanizationThreshold: 1, radiationThreshold: 1);
    }

    public string SampleBiomeTile(int x, int y)
    {
        return "base.grass";
    }
}

return new BaseBiome();

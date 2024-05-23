using System;

namespace MonoGame;

public interface IBiome
{
    string Id { get; set; }
    string Name { get; set; }
    bool Enabled { get; set; }
    BiomeGenerationConditions BiomeGenerationConditions { get; set; }
    public string SampleBiomeTile(int x, int y);
}

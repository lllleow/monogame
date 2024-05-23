using System;

namespace MonoGame;

public class BiomeGenerationConditions
{
    public double TemperatureThreshold { get; set; }
    public double ElevationThreshold { get; set; }
    public double UrbanizationThreshold { get; set; }
    public double RadiationThreshold { get; set; }

    public BiomeGenerationConditions(double temperatureThreshold, double elevationThreshold, double urbanizationThreshold, double radiationThreshold)
    {
        TemperatureThreshold = temperatureThreshold;
        ElevationThreshold = elevationThreshold;
        UrbanizationThreshold = urbanizationThreshold;
        RadiationThreshold = radiationThreshold;
    }
}

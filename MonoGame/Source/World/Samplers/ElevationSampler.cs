using System;
using DotnetNoise;

namespace MonoGame;

public class ElevationSampler : ISampler
{
    FastNoise fastNoise = new FastNoise(seed: new Random().Next());

    public double Sample(double x, double y)
    {
        // Parameters for noise generation
        float continentScale = 2000f; // Scale for large features
        float detailScale = 500f; // Scale for detailed features
        float baseAmplitude = 0.5f; // Amplitude for broad features
        float detailAmplitude = 0.25f; // Amplitude for detailed features

        // Generate base continent noise
        double baseContinent = fastNoise.GetPerlin((float)x * continentScale, (float)y * continentScale) * baseAmplitude;

        // Generate detailed noise
        double detailNoise = fastNoise.GetPerlin((float)x * detailScale, (float)y * detailScale) * detailAmplitude;

        // Calculate the combined amplitude to normalize properly
        double maxAmplitude = baseAmplitude + detailAmplitude;
        double minAmplitude = -maxAmplitude;

        // Combine noises
        double combinedSample = baseContinent + detailNoise;

        // Normalize to [0, 1] range
        combinedSample = (combinedSample - minAmplitude) / (maxAmplitude - minAmplitude);

        return combinedSample;
    }
}

using System;
using DotnetNoise;

namespace MonoGame;

public class ElevationSampler : ISampler
{
    FastNoise fastNoise = new FastNoise(seed: new Random().Next());
    public ElevationSampler() {
        fastNoise.Frequency = 0.001f;
    }

    public double Sample(double x, double y)
    {
        float scale = 100f; 
        double sample = fastNoise.GetPerlin((float)x * scale, (float)y * scale);
        return (sample + 1) / 2.0;
    }
}

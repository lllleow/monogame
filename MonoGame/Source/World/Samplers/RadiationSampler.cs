using System;
using DotnetNoise;

namespace MonoGame;

public class RadiationSampler : ISampler
{
    FastNoise fastNoise = new FastNoise(seed: new Random().Next());
    public float scale = 20f;

    public double Sample(double x, double y)
    {
        float xCoord = (float)(x / Chunk.SizeX * scale);
        float yCoord = (float)(y / Chunk.SizeY * scale);
        double sample = fastNoise.GetNoise(xCoord, yCoord);
        return (sample + 1) / 2.0;
    }
}

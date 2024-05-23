using System;
using DotnetNoise;

namespace MonoGame;

public class TemperatureSampler : ISampler
{
    FastNoise fastNoise = new FastNoise(seed: new Random().Next());

    public double Sample(double x, double y)
    {
        float xCoord = (float)(x / Chunk.SizeX);
        float yCoord = (float)(y / Chunk.SizeY);
        double sample = fastNoise.GetNoise(xCoord, yCoord);
        return (sample + 1) / 2.0;
    }
}

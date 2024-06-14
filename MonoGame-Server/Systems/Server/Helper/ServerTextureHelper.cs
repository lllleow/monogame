using System.Collections.Concurrent;
using System.Drawing;
using MonoGame_Common;
using MonoGame_Common.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MonoGame_Server.Systems.Server.Helper;

public static class ServerTextureHelper
{
    public static Dictionary<string, bool[,]> TextureMasks { get; set; } = [];
    public static ConcurrentDictionary<string, Image<Rgba32>> Textures { get; set; } = [];

    public static bool[,] GetImageMask(Image<Rgba32> image)
    {
        var width = image.Width;
        var height = image.Height;
        var mask = new bool[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var pixel = image[x, y];
                mask[x, y] = pixel.A > 0;
            }
        }

        return mask;
    }

    public static bool[,] GetImageMaskForRectangle(string image, System.Drawing.Rectangle rectangle)
    {
        Image<Rgba32> croppedImage = GetImageInRectangle(image, rectangle);
        return GetImageMask(croppedImage);
    }

    public static Image<Rgba32> GetImage(string imagePath)
    {
        if (Textures.TryGetValue(imagePath, out var value))
        {
            return value.Clone();
        }
        else
        {
            var image = SixLabors.ImageSharp.Image.Load<Rgba32>("../Assets/" + imagePath + ".png");
            Textures.TryAdd(imagePath, image.Clone());

            return image;
        }
    }

    public static Image<Rgba32> GetImageInRectangle(string imagePath, System.Drawing.Rectangle rectangle)
    {
        try
        {
            var image = GetImage(imagePath);
            Image<Rgba32> croppedImage = image.Clone(img => img.Crop(new SixLabors.ImageSharp.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)));
            return croppedImage;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading texture data for " + imagePath + " " + e);
            return new Image<Rgba32>(1, 1);
        }
    }

    public static Image<Rgba32> GetImageInCoordinates(string imagePath, int x, int y, int sizeX, int sizeY)
    {
        return GetImageInRectangle(imagePath, new System.Drawing.Rectangle(x * SharedGlobals.PixelSizeX, y * SharedGlobals.PixelSizeY, sizeX * SharedGlobals.PixelSizeX, sizeY * SharedGlobals.PixelSizeY));
    }
}

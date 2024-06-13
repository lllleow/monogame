using MonoGame_Common.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MonoGame_Server.Systems.Server.Helper;

public static class ServerTextureHelper
{
    public static Dictionary<string, bool[,]> TextureMasks { get; set; } = [];
    public static Dictionary<string, Image<Rgba32>> Textures { get; set; } = [];

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
        if (Textures.ContainsKey(imagePath))
        {
            return Textures[imagePath].Clone();
        }

        var image = Image.Load<Rgba32>("../Assets/" + imagePath + ".png");
        Textures.Add(imagePath, image.Clone());

        return image;
    }

    public static Image<Rgba32> GetImageInRectangle(string imagePath, System.Drawing.Rectangle rectangle)
    {
        var image = GetImage(imagePath);
        Image<Rgba32> croppedImage = image.Clone(img => img.Crop(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)));
        return croppedImage;
    }

    public static Image<Rgba32> GetImageInCoordinates(string imagePath, int x, int y, int sizeX, int sizeY)
    {
        return GetImageInRectangle(imagePath, new System.Drawing.Rectangle(x * TileState.PixelSizeX, y * TileState.PixelSizeY, sizeX * TileState.PixelSizeX, sizeY * TileState.PixelSizeY));
    }
}

using MonoGame_Common.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MonoGame_Server;

public static class ServerTextureHelper
{
    public static Dictionary<string, bool[,]> TextureMasks { get; set; } = new();
    public static Dictionary<string, Image> Textures { get; set; } = new();

    public static bool[,] GetImageMask(Image<Rgba32> image)
    {
        int width = image.Width;
        int height = image.Height;
        bool[,] mask = new bool[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Rgba32 pixel = image[x, y];
                mask[x, y] = pixel.A > 0;
            }
        }

        return mask;
    }

    public static Image GetImage(string imagePath)
    {
        if (Textures.ContainsKey(imagePath))
        {
            return Textures[imagePath];
        }

        using (Image image = Image.Load(imagePath))
        {
            Textures.Add(imagePath, image);
            return image;
        }
    }

    public static Image GetImageInRectangle(string imagePath, System.Drawing.Rectangle rectangle)
    {
        Image image = GetImage(imagePath);
        Image textureImage = image.Clone(image => image.Crop(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)));
        return textureImage;
    }

    public static Image GetImageInCoordinates(string imagePath, int x, int y, int sizeX, int sizeY)
    {
        return GetImageInRectangle(imagePath, new System.Drawing.Rectangle(x * TileState.PixelSizeX, y * TileState.PixelSizeY, sizeX * TileState.PixelSizeX, sizeY * TileState.PixelSizeY));
    }
}

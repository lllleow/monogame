using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MonoGame_Server;

public static class ImageLoader
{
    public static Dictionary<string, Image<Rgba32>> Images { get; set; } = [];

    public static Image<Rgba32> LoadImage(string path)
    {
        if (Images.ContainsKey(path))
        {
            return Images[path];
        }

        var image = Image.Load<Rgba32>(path);
        Images.Add(path, image);

        return image;
    }
}

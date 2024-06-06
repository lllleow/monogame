
using System.IO;
namespace MonoGame.Source.Util.Loaders;

public static class FileLoader
{

    public static string[] LoadAllFilesFromFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"The specified directory was not found: {folderPath}");
        }

        string[] files = Directory.GetFiles(folderPath);
        return files;
    }
}

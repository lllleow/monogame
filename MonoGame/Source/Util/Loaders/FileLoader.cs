
using System.IO;
namespace MonoGame.Source.Util.Loaders;

/// <summary>
/// Provides methods for loading files from a folder.
/// </summary>
public static class FileLoader
{
    /// <summary>
    /// Loads all files from the specified folder path.
    /// </summary>
    /// <param name="folderPath">The path of the folder to load files from.</param>
    /// <returns>An array of strings representing the file paths.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown if the specified directory does not exist.</exception>
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

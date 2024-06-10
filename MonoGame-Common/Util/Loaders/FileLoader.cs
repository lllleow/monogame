namespace MonoGame_Common.Util.Loaders;

public static class FileLoader
{
    public static string[] LoadAllFilesFromFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"The specified directory was not found: {folderPath}");
        }

        var files = Directory.GetFiles(folderPath);
        return files;
    }
}

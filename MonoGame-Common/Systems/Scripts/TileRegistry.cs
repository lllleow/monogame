using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Loaders;

namespace MonoGame_Common.Systems.Scripts;

public static class TileRegistry
{
    public static Dictionary<string, Type> Tiles { get; } = [];

    public static void RegisterTile(string id, Type tileType)
    {
        if (!typeof(CommonTile).IsAssignableFrom(tileType))
        {
            throw new ArgumentException("Tile type must implement ITile interface", nameof(tileType));
        }

        Tiles.Add(id, tileType);
    }

    public static CommonTile GetTile(string id)
    {
        var tileType = Tiles[id];
        var tile = Activator.CreateInstance(tileType) as CommonTile;
        return tile;
    }

    public static Type GetTileType(string id)
    {
        var tileType = Tiles[id];
        return tileType;
    }

    public static void LoadTileScripts()
    {
        var files = FileLoader.LoadAllFilesFromFolder(@"Scripts\Tiles");
        foreach (var file in files)
        {
            var code = File.ReadAllText(file);
            var tile = LoadTileScript(code);

            RegisterTile(tile.Id, tile.GetType());
        }
    }

    public static CommonTile LoadTileScript(string code)
    {
        var options = ScriptOptions.Default
            .AddReferences(Assembly.GetExecutingAssembly())
            .AddImports("MonoGame_Common");

        try
        {
            var tile = CSharpScript.EvaluateAsync<CommonTile>(code, options).Result;
            return tile;
        }
        catch (CompilationErrorException e)
        {
            Console.WriteLine("Compilation error: " + e.Message);
            throw;
        }
    }
}
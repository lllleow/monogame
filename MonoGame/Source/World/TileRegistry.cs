using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;

namespace MonoGame;

public static class TileRegistry
{
    public static Dictionary<string, Type> Tiles { get; private set; } = new Dictionary<string, Type>();

    public static void RegisterTile(string id, Type tileType)
    {
        if (!typeof(ITile).IsAssignableFrom(tileType))
        {
            throw new ArgumentException("Tile type must implement ITile interface", nameof(tileType));
        }
        Tiles.Add(id, tileType);
    }

    public static ITile GetTile(string id)
    {
        Type tileType = Tiles[id];
        ITile tile = Activator.CreateInstance(tileType) as ITile;
        return tile;
    }

    public static void LoadTileScripts()
    {
        string[] files = FileLoader.LoadAllFilesFromFolder(@"C:\Users\Leonardo\Documents\Repositories\monogame\MonoGame\Scripts\Tiles");
        foreach (string file in files)
        {
            string code = File.ReadAllText(file);
            ITile tile = LoadTileScript(code);
            RegisterTile(tile.Id, tile.GetType());
        }
    }

    public static ITile LoadTileScript(string code)
    {
        ScriptOptions options = ScriptOptions.Default
            .AddReferences(Assembly.GetExecutingAssembly())
            .AddImports("MonoGame");

        try
        {
            ITile tile = CSharpScript.EvaluateAsync<ITile>(code, options).Result;
            return tile;
        }
        catch (CompilationErrorException e)
        {
            Console.WriteLine("Compilation error: " + e.Message);
            throw;
        }
    }
}

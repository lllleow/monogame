using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;
using MonoGame.Source.Util.Loaders;

namespace MonoGame.Source.Systems.Scripts;

public static class TileRegistry
{
    /// <summary>
    /// Dictionary that stores the registered tiles.
    /// </summary>
    public static Dictionary<string, Type> Tiles { get; private set; } = new Dictionary<string, Type>();

    /// <summary>
    /// Registers a tile with the specified ID and tile type.
    /// </summary>
    /// <param name="id">The ID of the tile.</param>
    /// <param name="tileType">The type of the tile.</param>
    /// <exception cref="ArgumentException">Thrown if the tile type does not implement the ITile interface.</exception>
    public static void RegisterTile(string id, Type tileType)
    {
        if (!typeof(ITile).IsAssignableFrom(tileType))
        {
            throw new ArgumentException("Tile type must implement ITile interface", nameof(tileType));
        }
        Tiles.Add(id, tileType);
    }

    /// <summary>
    /// Retrieves a tile instance with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the tile.</param>
    /// <returns>The tile instance.</returns>
    public static ITile GetTile(string id)
    {
        Type tileType = Tiles[id];
        ITile tile = Activator.CreateInstance(tileType) as ITile;
        return tile;
    }

    /// <summary>
    /// Retrieves the type of the tile with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the tile.</param>
    /// <returns>The type of the tile.</returns>
    public static Type GetTileType(string id)
    {
        Type tileType = Tiles[id];
        return tileType;
    }

    /// <summary>
    /// Loads and registers tile scripts from a specified folder.
    /// </summary>
    public static void LoadTileScripts()
    {
        string[] files = FileLoader.LoadAllFilesFromFolder(@"Scripts\Tiles");
        foreach (string file in files)
        {
            string code = File.ReadAllText(file);
            ITile tile = LoadTileScript(code);
            RegisterTile(tile.Id, tile.GetType());
        }
    }

    /// <summary>
    /// Loads and evaluates a tile script.
    /// </summary>
    /// <param name="code">The code of the tile script.</param>
    /// <returns>The loaded tile instance.</returns>
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

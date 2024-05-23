using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;

namespace MonoGame;

public static class BiomeRegistry
{
    public static Dictionary<string, Type> Biomes { get; private set; } = new Dictionary<string, Type>();
    public static Dictionary<string, double> Thresholds { get; private set; } = new Dictionary<string, double>();

    public static void RegisterBiome(string id, Type biomeType)
    {
        if (!typeof(IBiome).IsAssignableFrom(biomeType))
        {
            throw new ArgumentException("Biome type must implement IBiome interface", nameof(biomeType));
        }
        Biomes.Add(id, biomeType);
    }

    public static IBiome GetBiome(string id)
    {
        IBiome tile = Activator.CreateInstance(GetBiomeType(id)) as IBiome;
        return tile;
    }

    public static Type GetBiomeType(string id)
    {
        Type type = Biomes[id];
        return type;
    }

    public static void LoadBiomeScripts()
    {
        string[] files = FileLoader.LoadAllFilesFromFolder(@"C:\Users\Leonardo\Documents\Repositories\monogame\MonoGame\Scripts\Biomes");
        foreach (string file in files)
        {
            string code = File.ReadAllText(file);
            IBiome biome = LoadBiomeScript(code);
            RegisterBiome(biome.Id, biome.GetType());
        }
    }

    public static IBiome LoadBiomeScript(string code)
    {
        ScriptOptions options = ScriptOptions.Default
            .AddReferences(Assembly.GetExecutingAssembly())
            .AddImports("MonoGame");

        try
        {
            IBiome biome = CSharpScript.EvaluateAsync<IBiome>(code, options).Result;
            return biome;
        }
        catch (CompilationErrorException e)
        {
            Console.WriteLine("Compilation error: " + e.Message);
            throw;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;

namespace MonoGame;

public static class AnimationBundleRegistry
{
    public static Dictionary<string, Type> AnimationBundles { get; private set; } = new Dictionary<string, Type>();

    public static void RegisterAnimationBundle(string id, Type tileType)
    {
        if (!typeof(IAnimationBundle).IsAssignableFrom(tileType))
        {
            throw new ArgumentException("Tile type must implement IAnimation interface", nameof(tileType));
        }
        AnimationBundles.Add(id, tileType);
    }

    public static IAnimationBundle GetAnimationBundle(string id)
    {
        Type animationType = AnimationBundles[id];
        IAnimationBundle animation = Activator.CreateInstance(animationType) as IAnimationBundle;
        return animation;
    }

    public static void LoadAnimationBundleScripts()
    {
        string[] files = FileLoader.LoadAllFilesFromFolder(@"C:\Users\Leonardo\Documents\Repositories\monogame\MonoGame\Scripts\AnimationBundles");
        foreach (string file in files)
        {
            string code = File.ReadAllText(file);
            IAnimationBundle animation = LoadAnimationBundleScript(code);
            RegisterAnimationBundle(animation.Id, animation.GetType());
        }
    }

    public static IAnimationBundle LoadAnimationBundleScript(string code)
    {
        ScriptOptions options = ScriptOptions.Default
            .AddReferences(Assembly.GetExecutingAssembly())
            .AddImports("MonoGame");

        try
        {
            IAnimationBundle tile = CSharpScript.EvaluateAsync<IAnimationBundle>(code, options).Result;
            return tile;
        }
        catch (CompilationErrorException e)
        {
            Console.WriteLine("Compilation error: " + e.Message);
            throw;
        }
    }
}

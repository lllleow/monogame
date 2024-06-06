using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using MonoGame.Source.Systems.Animation;
using MonoGame.Source.Util.Loaders;

namespace MonoGame.Source.Systems.Scripts;

public static class AnimationBundleRegistry
{
    public static Dictionary<string, Type> AnimationBundles { get; private set; } = [];

    public static void RegisterAnimationBundle(string id, Type bundleType)
    {
        if (!typeof(IAnimationBundle).IsAssignableFrom(bundleType))
        {
            throw new ArgumentException("Bundle type must implement IAnimationBundle interface", nameof(bundleType));
        }

        AnimationBundles.Add(id, bundleType);
    }

    public static IAnimationBundle GetAnimationBundle(string id)
    {
        Type bundleType = AnimationBundles[id];
        IAnimationBundle bundle = Activator.CreateInstance(bundleType) as IAnimationBundle;
        return bundle;
    }

    public static void LoadAnimationBundleScripts()
    {
        string[] files = FileLoader.LoadAllFilesFromFolder(@"Scripts\AnimationBundles");
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
            IAnimationBundle bundle = CSharpScript.EvaluateAsync<IAnimationBundle>(code, options).Result;
            return bundle;
        }
        catch (CompilationErrorException e)
        {
            Console.WriteLine("Compilation error: " + e.Message);
            throw;
        }
    }
}

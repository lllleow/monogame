using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;
using MonoGame.Source.Util.Loaders;

namespace MonoGame.Source.Systems.Scripts;

/// <summary>
/// Provides a registry for managing animation bundles in the application.
/// </summary>
public static class AnimationBundleRegistry
{
    /// <summary>
    /// Gets the dictionary of animation bundles, where the key is the bundle ID and the value is the bundle type.
    /// </summary>
    public static Dictionary<string, Type> AnimationBundles { get; private set; } = new Dictionary<string, Type>();

    /// <summary>
    /// Registers an animation bundle with the specified ID and type.
    /// </summary>
    /// <param name="id">The ID of the animation bundle.</param>
    /// <param name="bundleType">The type of the animation bundle.</param>
    /// <exception cref="ArgumentException">Thrown if the bundle type does not implement the IAnimationBundle interface.</exception>
    public static void RegisterAnimationBundle(string id, Type bundleType)
    {
        if (!typeof(IAnimationBundle).IsAssignableFrom(bundleType))
        {
            throw new ArgumentException("Bundle type must implement IAnimationBundle interface", nameof(bundleType));
        }
        AnimationBundles.Add(id, bundleType);
    }

    /// <summary>
    /// Retrieves the animation bundle with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the animation bundle.</param>
    /// <returns>The animation bundle with the specified ID.</returns>
    public static IAnimationBundle GetAnimationBundle(string id)
    {
        Type bundleType = AnimationBundles[id];
        IAnimationBundle bundle = Activator.CreateInstance(bundleType) as IAnimationBundle;
        return bundle;
    }

    /// <summary>
    /// Loads all animation bundle scripts from a specified folder.
    /// </summary>
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

    /// <summary>
    /// Loads an animation bundle script from the specified code.
    /// </summary>
    /// <param name="code">The code of the animation bundle script.</param>
    /// <returns>The loaded animation bundle.</returns>
    /// <exception cref="CompilationErrorException">Thrown if there is a compilation error in the script code.</exception>
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

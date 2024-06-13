using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using MonoGame_Common.Systems.Animation;
using MonoGame_Common.Util.Loaders;

namespace MonoGame_Common.Systems.Scripts;

public static class AnimationBundleRegistry
{
    public static Dictionary<string, Type> AnimationBundles { get; } = [];

    public static void RegisterAnimationBundle(string id, Type bundleType)
    {
        if (!typeof(IAnimationBundle).IsAssignableFrom(bundleType))
        {
            throw new ArgumentException("Bundle type must implement IAnimationBundle interface", nameof(bundleType));
        }

        AnimationBundles.Add(id, bundleType);
    }

    public static IAnimationBundle? GetAnimationBundle(string id)
    {
        var bundleType = AnimationBundles[id];
        var bundle = Activator.CreateInstance(bundleType) as IAnimationBundle;
        return bundle;
    }

    public static void LoadAnimationBundleScripts()
    {
        var folderPath = Path.Combine(SharedGlobals.ScriptsLocation, "AnimationBundles");
        var files = Directory.GetFiles(folderPath);
        foreach (var file in files)
        {
            var code = File.ReadAllText(file);
            var animation = LoadAnimationBundleScript(code);
            if (animation == null || animation.Id == null) continue;
            RegisterAnimationBundle(animation.Id, animation.GetType());
        }
    }

    public static IAnimationBundle LoadAnimationBundleScript(string code)
    {
        var options = ScriptOptions.Default
            .AddReferences(typeof(IAnimationBundle).Assembly)
            .AddImports("MonoGame_Common");

        try
        {
            var bundle = CSharpScript.EvaluateAsync<IAnimationBundle>(code, options).Result;
            return bundle;
        }
        catch (CompilationErrorException e)
        {
            Console.WriteLine("Compilation error: " + e.Message);
            throw;
        }
    }
}
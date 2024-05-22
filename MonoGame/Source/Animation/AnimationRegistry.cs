using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;

namespace MonoGame;

public static class AnimationRegistry
{
    public static Dictionary<string, Type> Animations { get; private set; } = new Dictionary<string, Type>();

    public static void RegisterAnimation(string id, Type tileType)
    {
        if (!typeof(IAnimation).IsAssignableFrom(tileType))
        {
            throw new ArgumentException("Tile type must implement IAnimation interface", nameof(tileType));
        }
        Animations.Add(id, tileType);
    }

    public static IAnimation GetAnimation(string id)
    {
        Type animationType = Animations[id];
        IAnimation animation = Activator.CreateInstance(animationType) as IAnimation;
        return animation;
    }

    public static void LoadAnimationScripts()
    {
        string[] files = FileLoader.LoadAllFilesFromFolder(@"C:\Users\Leonardo\Documents\Repositories\monogame\MonoGame\Scripts\Animations");
        foreach (string file in files)
        {
            string code = File.ReadAllText(file);
            IAnimation animation = LoadAnimationScript(code);
            RegisterAnimation(animation.Id, animation.GetType());
        }
    }

    public static IAnimation LoadAnimationScript(string code)
    {
        ScriptOptions options = ScriptOptions.Default
            .AddReferences(Assembly.GetExecutingAssembly())
            .AddImports("MonoGame");

        try
        {
            IAnimation tile = CSharpScript.EvaluateAsync<IAnimation>(code, options).Result;
            return tile;
        }
        catch (CompilationErrorException e)
        {
            Console.WriteLine("Compilation error: " + e.Message);
            throw;
        }
    }
}

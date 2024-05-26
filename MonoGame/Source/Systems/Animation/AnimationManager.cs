using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame.Source.Systems.Animation;

public class AnimationManager
{
    private static List<Action<float>> activeAnimations = new List<Action<float>>();

    public static void Add(Action<float> animation)
    {
        activeAnimations.Add(animation);
    }

    public static void Remove(Action<float> animation)
    {
        activeAnimations.Remove(animation);
    }

    public static void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        for (int i = activeAnimations.Count - 1; i >= 0; i--)
        {
            activeAnimations[i](deltaTime);
        }
    }
}
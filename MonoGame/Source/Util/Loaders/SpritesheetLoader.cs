using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Util.Loaders;

public class SpritesheetLoader
{
    private static Dictionary<string, Texture2D> SpriteSheets = new Dictionary<string, Texture2D>();

    public static Texture2D GetSpritesheet(string spritesheet)
    {
        if (SpriteSheets.ContainsKey(spritesheet))
        {
            return SpriteSheets[spritesheet];
        }
        else
        {
            var texture = Globals.contentManager.Load<Texture2D>(spritesheet);
            SpriteSheets.Add(spritesheet, texture);
            return texture;
        }
    }
}


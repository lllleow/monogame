using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class SpritesheetLoader
{
    private static Dictionary<string, Texture2D> spritesheets = new Dictionary<string, Texture2D>();

    public static Texture2D GetSpritesheet(string spritesheet)
    {
        if (spritesheets.ContainsKey(spritesheet))
        {
            return spritesheets[spritesheet];
        }
        else
        {
            var texture = Globals.contentManager.Load<Texture2D>(spritesheet);
            spritesheets.Add(spritesheet, texture);
            return texture;
        }
    }
}

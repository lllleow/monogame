using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Utils.Loaders;

public class SpritesheetLoader
{
    private static readonly Dictionary<string, Texture2D> SpriteSheets = [];

    public static Texture2D GetSpritesheet(string spritesheet)
    {
        if (SpriteSheets.TryGetValue(spritesheet, out var value))
        {
            return value;
        }

        var texture = Globals.ContentManager.Load<Texture2D>(spritesheet);
        SpriteSheets.Add(spritesheet, texture);
        return texture;
    }
}
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Utils.Loaders;

public class SpritesheetLoader
{
    private static readonly Dictionary<string, Texture2D> SpriteSheets = [];

    public static Texture2D GetSpritesheet(string spritesheet)
    {
        if (SpriteSheets.ContainsKey(spritesheet))
        {
            return SpriteSheets[spritesheet];
        }

        var texture = Globals.ContentManager.Load<Texture2D>(spritesheet);
        SpriteSheets.Add(spritesheet, texture);
        return texture;
    }
}
using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Util.Loaders;

/// <summary>
/// A utility class for loading and caching spritesheets.
/// </summary>
public class SpritesheetLoader
{
    private static Dictionary<string, Texture2D> SpriteSheets = new Dictionary<string, Texture2D>();

    /// <summary>
    /// Gets the spritesheet texture associated with the specified spritesheet name.
    /// If the spritesheet is not already loaded, it will be loaded and cached for future use.
    /// </summary>
    /// <param name="spritesheet">The name of the spritesheet to load.</param>
    /// <returns>The spritesheet texture.</returns>
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


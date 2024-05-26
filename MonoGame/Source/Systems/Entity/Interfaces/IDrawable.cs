using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Systems.Entity.Interfaces;

/// <summary>
/// Represents an object that can be drawn on the screen.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Gets or sets the name of the spritesheet associated with the drawable object.
    /// </summary>
    string SpritesheetName { get; set; }

    /// <summary>
    /// Gets or sets the width of the drawable object in pixels.
    /// </summary>
    int PixelSizeX { get; set; }

    /// <summary>
    /// Gets or sets the height of the drawable object in pixels.
    /// </summary>
    int PixelSizeY { get; set; }

    /// <summary>
    /// Gets or sets the X-coordinate of the texture within the spritesheet.
    /// </summary>
    int TextureX { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate of the texture within the spritesheet.
    /// </summary>
    int TextureY { get; set; }

    /// <summary>
    /// Draws the drawable object using the specified SpriteBatch.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch used to draw the object.</param>
    void Draw(SpriteBatch spriteBatch);
}
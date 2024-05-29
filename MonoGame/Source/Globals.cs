using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.Camera;
namespace MonoGame;

/// <summary>
/// Represents a class that holds global variables and objects used throughout the application.
/// </summary>
public class Globals
{
    /// <summary>
    /// Gets or sets the content manager used for loading and managing game content.
    /// </summary>
    public static ContentManager contentManager { get; set; }

    /// <summary>
    /// Gets or sets the sprite batch used for rendering 2D graphics.
    /// </summary>
    public static SpriteBatch spriteBatch { get; set; }

    /// <summary>
    /// Gets or sets the world object representing the game world.
    /// </summary>
    public static World world { get; set; }

    /// <summary>
    /// Gets or sets the graphics device manager used for managing the graphics device.
    /// </summary>
    public static GraphicsDeviceManager graphicsDevice { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the game is running in full-screen mode.
    /// </summary>
    public static bool FullScreen { get; internal set; } = false;

    /// <summary>
    /// Gets or sets the camera object used for viewing the game world.
    /// </summary>
    public static Camera camera;

    /// <summary>
    /// Gets or sets the game object.
    /// </summary>
    public static Game game;

    public static UserInterfaceHandler userInterfaceHandler;

    public static void DefaultSpriteBatchBegin()
    {
        Globals.spriteBatch.Begin(transformMatrix: camera.Transform, sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
    }
}
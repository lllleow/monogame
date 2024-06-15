using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.Camera;
using MonoGame.Source.Rendering.UI;
using MonoGame.Source.WorldNamespace;

namespace MonoGame.Source;

public class Globals
{
    public static ContentManager ContentManager { get; set; }

    public static SpriteBatch SpriteBatch { get; set; }

    public static World World { get; set; }

    public static GraphicsDeviceManager GraphicsDevice { get; set; }

    public static bool FullScreen { get; internal set; } = false;

    public static Camera Camera { get; set; }

    public static Game Game { get; set; }

    public static string[] Args { get; set; }

    public static UserInterfaceHandler UserInterfaceHandler { get; set; }

    public static SpriteFont DefaultFont { get; set; }

    public static string UUID { get; set; } = Guid.NewGuid().ToString();

    public static GameTime GameTime { get; set; }

    public static bool ShowTileBoundingBox { get; set; } = false;
    public static InputManager InputManager { get; set; } = new();

    public static void DefaultSpriteBatchBegin()
    {
        SpriteBatch.Begin(transformMatrix: Camera.Transform, sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
    }

    internal static void DefaultSpriteBatchUIBegin()
    {
        SpriteBatch.Begin(transformMatrix: Globals.UserInterfaceHandler.Transform, sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
    }
}
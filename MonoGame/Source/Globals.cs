using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.Camera;
namespace MonoGame;

public class Globals
{

    public static ContentManager ContentManager { get; set; }

    public static SpriteBatch SpriteBatch { get; set; }

    public static World World { get; set; }

    public static GraphicsDeviceManager GraphicsDevice { get; set; }

    public static bool FullScreen { get; internal set; } = false;

    public static Camera Camera;

    public static Game Game;

    public static string[] Args;

    public static UserInterfaceHandler UserInterfaceHandler;

    public static SpriteFont DefaultFont;

    public static Vector2 SpawnPosition = new Vector2(128, 128);

    public static string UUID = Guid.NewGuid().ToString();

    public static GameTime gameTime;

    public static void DefaultSpriteBatchBegin()
    {
        Globals.SpriteBatch.Begin(transformMatrix: Camera.Transform, sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
    }
}
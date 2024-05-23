using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class Main : Game
{
    private Camera camera;
    private const int ScreenSizeX = 640;
    private const int ScreenSizeY = 480;
    private FrameCounter _frameCounter = new FrameCounter();

    public Main()
    {
        Globals.graphicsDevice = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Globals.graphicsDevice.PreferredBackBufferWidth = 640;
        Globals.graphicsDevice.PreferredBackBufferHeight = 480;
        Globals.graphicsDevice.PreferMultiSampling = false;
        Globals.graphicsDevice.ApplyChanges();

        camera = new Camera(ScreenSizeX, ScreenSizeY);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.contentManager = this.Content;
        Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

        TileRegistry.LoadTileScripts();
        AnimationBundleRegistry.LoadAnimationBundleScripts();
        BiomeRegistry.LoadBiomeScripts();

        Globals.world = new World();
        Globals.world.InitWorld();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Globals.world.Update(gameTime);
        camera.Follow(Globals.world.Player);
        camera.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _frameCounter.Update(deltaTime);

        var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
        Console.Write(fps);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        Globals.spriteBatch.Begin(transformMatrix: camera.Transform, sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
        Globals.world.Draw(Globals.spriteBatch);
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class Main : Game
{
    private const int ScreenSizeX = 1080;
    private const int ScreenSizeY = 720;
    private FrameCounter _frameCounter = new FrameCounter();

    public Main()
    {
        Globals.graphicsDevice = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Globals.graphicsDevice.PreferredBackBufferWidth = ScreenSizeX;
        Globals.graphicsDevice.PreferredBackBufferHeight = ScreenSizeY;
        Globals.graphicsDevice.PreferMultiSampling = false;
        // Globals.graphicsDevice.IsFullScreen = true;
        Globals.graphicsDevice.ApplyChanges();

        Globals.camera = new Camera(ScreenSizeX, ScreenSizeY);
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
        Globals.camera.Follow(Globals.world.Player, gameTime);
        Globals.camera.Update(gameTime);
        AnimationManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _frameCounter.Update(deltaTime);

        var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        Globals.spriteBatch.Begin(transformMatrix: Globals.camera.Transform, sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
        Globals.world.Draw(Globals.spriteBatch);
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}

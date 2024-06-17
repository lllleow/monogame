using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame_Common.Systems.Scripts;
using MonoGame.Source;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Rendering.Camera;
using MonoGame.Source.Rendering.UI;
using MonoGame.Source.WorldNamespace;

namespace MonoGame;

public class Main : Game
{
    private const int ScreenSizeX = 1080;
    private const int ScreenSizeY = 720;

    public Main()
    {
        Globals.GraphicsDevice = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Globals.Game = this;
        Globals.GraphicsDevice.PreferredBackBufferWidth = ScreenSizeX;
        Globals.GraphicsDevice.PreferredBackBufferHeight = ScreenSizeY;
        Globals.GraphicsDevice.PreferMultiSampling = false;
        Globals.GraphicsDevice.IsFullScreen = Globals.FullScreen;
        Globals.GraphicsDevice.ApplyChanges();

        Globals.Camera = new Camera(ScreenSizeX, ScreenSizeY);

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 60);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Globals.ContentManager = Content;
        Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.UserInterfaceHandler = new UserInterfaceHandler();
        Globals.DefaultFont = Content.Load<SpriteFont>("fonts/pixelify_sans/PixelifySans-Regular");

        TileRegistry.LoadTileScripts();
        AnimationBundleRegistry.LoadAnimationBundleScripts();

        Globals.World = new World();

        Globals.UserInterfaceHandler.Initialize();
        Globals.World.UpdateAllTextureCoordinates();
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        base.OnExiting(sender, args);
    }

    private int frameCount = 0;
    private float elapsedTime = 0f;
    private string windowTitle = "MonoGame";

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        // Update frame count and elapsed time
        frameCount++;
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update window title every few cycles
        if (elapsedTime >= 1f)
        {
            Window.Title = $"{windowTitle} - FPS: {frameCount / elapsedTime}";
            frameCount = 0;
            elapsedTime = 0f;
        }

        Globals.GameTime = gameTime;
        Globals.InputManager.Update();
        NetworkClient.Update();
        Globals.World.Update(gameTime);

        Camera.Update(gameTime);
        Globals.UserInterfaceHandler.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(30, 188, 115, 1));

        Globals.DefaultSpriteBatchBegin();
        Globals.World.Draw(Globals.SpriteBatch);
        Globals.SpriteBatch.End();

        Globals.DefaultSpriteBatchUIBegin();
        Globals.UserInterfaceHandler.Draw(Globals.SpriteBatch);
        Globals.SpriteBatch.End();

        base.Draw(gameTime);
    }
}
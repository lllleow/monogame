using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Rendering.Camera;
using MonoGame.Source.Rendering.UI;
using MonoGame.Source.Systems.Scripts;
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

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        Globals.GameTime = gameTime;
        NetworkClient.Update();
        Globals.World.Update(gameTime);
        Globals.Camera.Follow(Globals.World.GetLocalPlayer(), gameTime);
        Globals.Camera.Update(gameTime);
        Globals.UserInterfaceHandler.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Globals.DefaultSpriteBatchBegin();
        Globals.World.Draw(Globals.SpriteBatch);
        Globals.SpriteBatch.End();

        Globals.SpriteBatch.Begin(transformMatrix: Globals.UserInterfaceHandler.Transform, sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
        Globals.UserInterfaceHandler.Draw(Globals.SpriteBatch);
        Globals.SpriteBatch.End();

        base.Draw(gameTime);
    }
}
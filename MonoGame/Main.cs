using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Rendering.Camera;
using MonoGame.Source.Systems.Scripts;

namespace MonoGame;

/// <summary>
/// Represents the main game class.
/// </summary>
public class Main : Game
{
    private const int ScreenSizeX = 1080;
    private const int ScreenSizeY = 720;

    public Main()
    {
        Globals.graphicsDevice = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Globals.game = this;
        Globals.graphicsDevice.PreferredBackBufferWidth = ScreenSizeX;
        Globals.graphicsDevice.PreferredBackBufferHeight = ScreenSizeY;
        Globals.graphicsDevice.PreferMultiSampling = false;
        Globals.graphicsDevice.IsFullScreen = Globals.FullScreen;
        Globals.graphicsDevice.ApplyChanges();

        Globals.camera = new Camera(ScreenSizeX, ScreenSizeY);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        NetworkMode mode = NetworkMode.Server;
        if (Globals.args.Length > 0)
        {
            if (Globals.args[0] == "client")
            {
                mode = NetworkMode.Client;
            }
        }

        Window.Title = mode.ToString() + " - " + "MonoGame";

        Globals.contentManager = this.Content;
        Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.userInterfaceHandler = new UserInterfaceHandler();
        Globals.networkManager = new NetworkManager();
        Globals.defaultFont = Content.Load<SpriteFont>("PixelifySans");

        Globals.networkManager.Initialize();

        TileRegistry.LoadTileScripts();
        AnimationBundleRegistry.LoadAnimationBundleScripts();

        if (!SaveManager.LoadGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\") && Globals.networkMode != NetworkMode.Client)
        {
            Globals.world = new World();
            Globals.world.InitWorld();
        }

        Globals.userInterfaceHandler.Initialize();
        Globals.world.UpdateAllTextureCoordinates();
    }

    protected override void OnExiting(Object sender, EventArgs args)
    {
        SaveManager.SaveGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\");
        base.OnExiting(sender, args);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Globals.networkManager.Update();
        Globals.world.Update(gameTime);
        Globals.camera.Follow(Globals.world.Players.First(), gameTime);
        Globals.camera.Update(gameTime);
        Globals.userInterfaceHandler.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Globals.DefaultSpriteBatchBegin();
        Globals.world.Draw(Globals.spriteBatch);
        Globals.spriteBatch.End();

        Globals.spriteBatch.Begin(transformMatrix: Globals.userInterfaceHandler.Transform, sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
        Globals.userInterfaceHandler.Draw(Globals.spriteBatch);
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}

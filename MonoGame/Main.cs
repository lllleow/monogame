using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class Main : Game
{
    private World world;
    private const float scaleFactor = 2.5f;
    private Matrix globalScale = Matrix.CreateScale(scaleFactor, scaleFactor, 1f);

    public Main()
    {
        Globals.graphicsDevice = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Globals.graphicsDevice.PreferredBackBufferWidth = 640;
        Globals.graphicsDevice.PreferredBackBufferHeight = 480;
        Globals.graphicsDevice.PreferMultiSampling = false;
        Globals.graphicsDevice.ApplyChanges();
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

        world = new World();
        world.InitWorld();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        world.Update(gameTime);
        base.Update(gameTime);
    }

    Matrix viewMatrix;
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);


        Vector2 screenCenter = new Vector2(320, 240);
        Vector2 translation = screenCenter - world.player.Position * 1.5f;
        viewMatrix = Matrix.CreateTranslation(new Vector3(translation, 0f));

        Globals.spriteBatch.Begin(transformMatrix: globalScale * viewMatrix, sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

        world.Draw(Globals.spriteBatch);
        Globals.spriteBatch.End();

        base.Draw(gameTime);
    }
}

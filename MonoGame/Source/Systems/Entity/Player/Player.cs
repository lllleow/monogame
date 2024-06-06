using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Entity;
using MonoGame.Source.Systems.Scripts;
namespace MonoGame;

public class Player : GameEntity
{
    AnimatorComponent Animator;
    SpriteRendererComponent SpriteRenderer;
    MouseState currentMouseState;
    MouseState previousMouseState;
    public string selectedTile = "base.grass";

    public Player(string uuid, Vector2 position)
    {
        Position = position;
        Speed = new Vector2(1, 1);
        UUID = uuid;

        Animator = new AnimatorComponent(this, AnimationBundleRegistry.GetAnimationBundle("base.player"));
        SpriteRenderer = new SpriteRendererComponent();

        AddComponent(SpriteRenderer);
        AddComponent(Animator);
        AddComponent(new PixelBoundsComponent());
        AddComponent(new CollisionComponent("textures/player_sprite_2_collision_mask"));
    }

    public void SetSelectedTile(string selectedTileId)
    {
        selectedTile = selectedTileId;
    }

    public bool IsLocalPlayer()
    {
        return this == Globals.World.GetLocalPlayer();
    }

    Vector2 lastPosition = Vector2.Zero;
    private void HandleMouseClick(bool add, int x, int y)
    {
        int windowWidth = Globals.GraphicsDevice.PreferredBackBufferWidth;
        int windowHeight = Globals.GraphicsDevice.PreferredBackBufferHeight;

        if (!Globals.Game.IsActive)
        {
            return;
        }

        if (x < 0 || y < 0 || x >= windowWidth || y >= windowHeight)
        {
            return;
        }

        Vector2 screenPosition = new Vector2(x, y);
        (int, int) globalPosition = Globals.World.GetGlobalPositionFromScreenPosition(screenPosition);

        if (screenPosition != lastPosition)
        {
            lastPosition = screenPosition;
            NetworkClient.Instance.SendMessage(new RequestToPlaceTileNetworkMessage(selectedTile, TileDrawLayer.Tiles, (int)globalPosition.Item1, (int)globalPosition.Item2));
        }
    }

    bool clicked = false;
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (this == Globals.World.GetLocalPlayer())
        {
            KeyboardState state = Keyboard.GetState();

            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Pressed || currentMouseState.RightButton == ButtonState.Pressed)
            {
                if (!clicked) clicked = true;
            }

            if (currentMouseState.LeftButton == ButtonState.Released)
            {
                if (clicked)
                {
                    int mouseX = currentMouseState.X;
                    int mouseY = currentMouseState.Y;
                    HandleMouseClick(currentMouseState.LeftButton == ButtonState.Pressed, mouseX, mouseY);
                    clicked = false;
                }
            }

            previousMouseState = currentMouseState;
            if (state.IsKeyDown(Keys.W))
            {
                NetworkClient.Instance.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Up));
            }
            if (state.IsKeyDown(Keys.A))
            {
                NetworkClient.Instance.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Left));
            }
            if (state.IsKeyDown(Keys.S))
            {
                NetworkClient.Instance.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Down));
            }
            if (state.IsKeyDown(Keys.D))
            {
                NetworkClient.Instance.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Right));
            }

            if (state.IsKeyUp(Keys.W) && state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.S) && state.IsKeyUp(Keys.D))
            {
                Animator?.PlayAnimation("idle");
            }
        }
        base.Update(gameTime);
    }
}

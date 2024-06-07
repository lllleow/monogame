using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Messages.Player;
using MonoGame.Source.Rendering.Enum;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.PixelBounds;
using MonoGame.Source.Systems.Components.SpriteRenderer;
using MonoGame.Source.Systems.Scripts;
using MonoGame.Source.Util.Enum;
namespace MonoGame.Source.Systems.Entity.PlayerNamespace;

public class Player : GameEntity
{
    private readonly AnimatorComponent animator;
    private readonly SpriteRendererComponent spriteRenderer;
    private MouseState currentMouseState;
    private MouseState previousMouseState;
    public string SelectedTile { get; set; } = "base.grass";

    public Player(string uuid, Vector2 position)
    {
        Position = position;
        Speed = new Vector2(1, 1);
        UUID = uuid;

        animator = new AnimatorComponent(this, AnimationBundleRegistry.GetAnimationBundle("base.player"));
        spriteRenderer = new SpriteRendererComponent();

        ClientNetworkEventManager.Subscribe<MovePlayerNetworkMessage>(message =>
        {
            if (UUID == message.UUID)
            {
                if (Vector2.Distance(Position, message.ExpectedPosition) < 1f)
                {
                    Position = message.ExpectedPosition;
                }
                else
                {
                    Move(Globals.GameTime, message.Direction, message.Speed);
                }
            }
        });

        ClientNetworkEventManager.Subscribe<UpdatePlayerPositionNetworkMessage>(message =>
        {
            if (UUID == message.UUID)
            {
                Position = message.Position;
            }
        });

        AddComponent(spriteRenderer);
        AddComponent(animator);
        AddComponent(new PixelBoundsComponent());
        AddComponent(new CollisionComponent("textures/player_sprite_2_collision_mask"));
    }

    public void SetSelectedTile(string selectedTileId)
    {
        SelectedTile = selectedTileId;
    }

    public bool IsLocalPlayer()
    {
        return this == Globals.World.GetLocalPlayer();
    }

    private Vector2 lastPosition = Vector2.Zero;
    private void HandleMouseClick(bool add, int x, int y)
    {
        var windowWidth = Globals.GraphicsDevice.PreferredBackBufferWidth;
        var windowHeight = Globals.GraphicsDevice.PreferredBackBufferHeight;

        if (!Globals.Game.IsActive)
        {
            return;
        }

        if (x < 0 || y < 0 || x >= windowWidth || y >= windowHeight)
        {
            return;
        }

        var screenPosition = new Vector2(x, y);
        var globalPosition = Globals.World.GetGlobalPositionFromScreenPosition(screenPosition);

        if (screenPosition != lastPosition)
        {
            lastPosition = screenPosition;
            NetworkClient.SendMessage(new RequestToPlaceTileNetworkMessage(SelectedTile, TileDrawLayer.Tiles, globalPosition.PosX, globalPosition.PosY));
        }
    }

    private bool clicked = false;
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (this == Globals.World.GetLocalPlayer())
        {
            var state = Keyboard.GetState();

            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Pressed || currentMouseState.RightButton == ButtonState.Pressed)
            {
                if (!clicked)
                {
                    clicked = true;
                }
            }

            if (currentMouseState.LeftButton == ButtonState.Released)
            {
                if (clicked)
                {
                    var mouseX = currentMouseState.X;
                    var mouseY = currentMouseState.Y;
                    HandleMouseClick(currentMouseState.LeftButton == ButtonState.Pressed, mouseX, mouseY);
                    clicked = false;
                }
            }

            previousMouseState = currentMouseState;
            if (state.IsKeyDown(Keys.W))
            {
                NetworkClient.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Up));
            }

            if (state.IsKeyDown(Keys.A))
            {
                NetworkClient.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Left));
            }

            if (state.IsKeyDown(Keys.S))
            {
                NetworkClient.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Down));
            }

            if (state.IsKeyDown(Keys.D))
            {
                NetworkClient.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Right));
            }

            if (state.IsKeyUp(Keys.W) && state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.S) && state.IsKeyUp(Keys.D))
            {
                animator?.PlayAnimation("idle");
            }
        }

        base.Update(gameTime);
    }
}

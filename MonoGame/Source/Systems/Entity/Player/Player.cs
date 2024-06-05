using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Entity;
using MonoGame.Source.Systems.Scripts;
namespace MonoGame;

/// <summary>
/// Represents a player entity in the game.
/// </summary>
public class Player : GameEntity
{
    AnimatorComponent Animator;
    SpriteRendererComponent SpriteRenderer;
    MouseState currentMouseState;
    MouseState previousMouseState;
    public string selectedTile = "base.grass";

    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    /// <param name="position">The initial position of the player.</param>
    public Player(string uuid, Vector2 position)
    {
        Position = position;
        Speed = new Vector2(1, 1);
        UUID = uuid;

        Animator = new AnimatorComponent(this, AnimationBundleRegistry.GetAnimationBundle("base.player"));
        SpriteRenderer = new SpriteRendererComponent();

        // AddComponent(new BoundingBoxComponent(new Vector2(16, 16)));
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
        return this == Globals.world.GetLocalPlayer();
    }

    /// <summary>
    /// Handles the mouse click event at the specified coordinates, ensuring it occurs within the window bounds and when the window is active.
    /// </summary>
    /// <param name="x">The x-coordinate of the mouse click.</param>
    /// <param name="y">The y-coordinate of the mouse click.</param>

    float lastLocalX = -1;
    float lastLocalY = -1;

    private void HandleMouseClick(bool add, int x, int y)
    {
        int windowWidth = Globals.graphicsDevice.PreferredBackBufferWidth;
        int windowHeight = Globals.graphicsDevice.PreferredBackBufferHeight;

        if (!Globals.game.IsActive)
        {
            return;
        }

        if (x < 0 || y < 0 || x >= windowWidth || y >= windowHeight)
        {
            return;
        }

        Vector2 worldPosition = new Vector2(x, y);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.camera.Transform));

        NetworkClient.Instance.SendMessage(new RequestToPlaceTileNetworkMessage(selectedTile, TileDrawLayer.Tiles, (int)worldPosition.X, (int)worldPosition.Y));
    }

    /// <summary>
    /// Updates the player's state based on user input and game time.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    bool clicked = false;
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
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

        if (IsLocalPlayer())
        {
            if (state.IsKeyUp(Keys.W) && state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.S) && state.IsKeyUp(Keys.D))
            {
                Animator?.PlayAnimation("idle");
            }
        }

        base.Update(gameTime);
    }
}

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

    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    /// <param name="position">The initial position of the player.</param>
    public Player(Vector2 position)
    {
        Position = position;
        Speed = new Vector2(1, 1);

        Animator = new AnimatorComponent(this, AnimationBundleRegistry.GetAnimationBundle("base.player"));
        SpriteRenderer = new SpriteRendererComponent();

        // AddComponent(new BoundingBoxComponent(new Vector2(16, 16)));
        AddComponent(SpriteRenderer);
        AddComponent(Animator);
        AddComponent(new PixelBoundsComponent());
        AddComponent(new CollisionComponent("textures/player_sprite_2_collision_mask"));
    }

    /// <summary>
    /// Handles the mouse click event at the specified coordinates, ensuring it occurs within the window bounds and when the window is active.
    /// </summary>
    /// <param name="x">The x-coordinate of the mouse click.</param>
    /// <param name="y">The y-coordinate of the mouse click.</param>
    private void HandleMouseClick(int x, int y)
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

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        IChunk chunk = Globals.world.CreateOrGetChunk(chunkX, chunkY);

        if (chunk.GetTile(TileDrawLayer.Tiles, localX, localY) != null)
        {
            chunk.DeleteTile(TileDrawLayer.Tiles, localX, localY);
        }
        else
        {
            if (chunk.GetTile(TileDrawLayer.Terrain, localX, localY) != null)
            {
                chunk.DeleteTile(TileDrawLayer.Terrain, localX, localY);
            }
            else
            {
                chunk.SetTileAndUpdateNeighbors("base.fence", TileDrawLayer.Tiles, localX, localY);
            }
        }
    }

    /// <summary>
    /// Updates the player's state based on user input and game time.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        KeyboardState state = Keyboard.GetState();

        currentMouseState = Mouse.GetState();

        if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
        {
            int mouseX = currentMouseState.X;
            int mouseY = currentMouseState.Y;

            HandleMouseClick(mouseX, mouseY);
        }

        previousMouseState = currentMouseState;

        if (state.IsKeyDown(Keys.W))
        {
            if (Move(gameTime, Direction.Up, Speed))
            {
                Animator?.PlayAnimation("walking_back");
            }
        }
        if (state.IsKeyDown(Keys.A))
        {
            if (Move(gameTime, Direction.Left, Speed))
            {
                Animator?.PlayAnimation("walking_left");
            }
        }
        if (state.IsKeyDown(Keys.S))
        {
            if (Move(gameTime, Direction.Down, Speed))
            {
                Animator?.PlayAnimation("walking_front");
            }
        }
        if (state.IsKeyDown(Keys.D))
        {
            if (Move(gameTime, Direction.Right, Speed))
            {
                Animator?.PlayAnimation("walking_right");
            }
        }

        if (state.IsKeyUp(Keys.W) && state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.S) && state.IsKeyUp(Keys.D))
        {
            Animator?.PlayAnimation("idle");
        }

        base.Update(gameTime);
    }
}

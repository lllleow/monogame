using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Rendering.Camera;

/// <summary>
/// Represents a camera used for rendering in a game.
/// </summary>
public class Camera
{
    /// <summary>
    /// Gets or sets the transformation matrix of the camera.
    /// </summary>
    public Matrix Transform { get; set; } = Matrix.Identity;

    private float ScaleFactor = 3f;
    private float ScreenSizeX { get; set; }
    private float ScreenSizeY { get; set; }
    private float FollowSpeed = 7f;
    private int previousScrollValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="Camera"/> class.
    /// </summary>
    /// <param name="screenSizeX">The width of the screen.</param>
    /// <param name="screenSizeY">The height of the screen.</param>
    public Camera(int screenSizeX, int screenSizeY)
    {
        ScreenSizeX = screenSizeX;
        ScreenSizeY = screenSizeY;
        previousScrollValue = Mouse.GetState().ScrollWheelValue;
    }

    /// <summary>
    /// Adjusts the camera's position to follow a specified game entity.
    /// </summary>
    /// <param name="entity">The game entity to follow.</param>
    /// <param name="gameTime">The current game time.</param>
    public void Follow(IGameEntity entity, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 entityCenter = new Vector2(entity.Position.X + Tile.PixelSizeX / 2, entity.Position.Y + Tile.PixelSizeY / 2);
        Vector2 screenCenter = new Vector2(ScreenSizeX / 2f, ScreenSizeY / 2f);

        Matrix targetTranslation = Matrix.CreateTranslation(
            screenCenter.X - entityCenter.X * ScaleFactor,
            screenCenter.Y - entityCenter.Y * ScaleFactor,
            0);

        Matrix targetTransform = Matrix.Multiply(Matrix.CreateScale(ScaleFactor, ScaleFactor, 1f), targetTranslation);

        Transform = Matrix.Lerp(Transform, targetTransform, FollowSpeed * deltaTime);
    }

    /// <summary>
    /// Updates the camera based on the current game time.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        int currentScrollValue = mouseState.ScrollWheelValue;

        if (currentScrollValue != previousScrollValue)
        {
            float delta = (currentScrollValue - previousScrollValue) / 120;
            ScaleFactor += delta * 0.1f;
            ScaleFactor = MathHelper.Clamp(ScaleFactor, 0.1f, 10f);

            previousScrollValue = currentScrollValue;
        }
    }
}


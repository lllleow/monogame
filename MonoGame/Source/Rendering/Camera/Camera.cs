using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Rendering.Camera;
public class Camera
{
    public Matrix Transform { get; set; } = Matrix.Identity;

    float ScaleFactor = 6f;
    float ScreenSizeX { get; set; }
    float ScreenSizeY { get; set; }
    private int previousScrollValue;

    public Camera(int screenSizeX, int screenSizeY)
    {
        ScreenSizeX = screenSizeX;
        ScreenSizeY = screenSizeY;
        previousScrollValue = Mouse.GetState().ScrollWheelValue;
    }

    private float followSpeed = 7f;
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

        Transform = Matrix.Lerp(Transform, targetTransform, followSpeed * deltaTime);
    }

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


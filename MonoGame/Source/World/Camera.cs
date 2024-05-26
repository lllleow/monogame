using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;
public class Camera
{
    public Matrix Transform { get; set; } = Matrix.Identity;

    float ScaleFactor = 2.5f;
    float ScreenSizeX { get; set; }
    float ScreenSizeY { get; set; }
    private int previousScrollValue;

    public Camera(int screenSizeX, int screenSizeY)
    {
        ScreenSizeX = screenSizeX;
        ScreenSizeY = screenSizeY;
        previousScrollValue = Mouse.GetState().ScrollWheelValue;
    }

    public void Follow(IGameEntity entity)
    {
        Vector2 entityCenter = new Vector2(entity.Position.X + 32 / 2, entity.Position.Y + 32 / 2);
        Vector2 screenCenter = new Vector2(ScreenSizeX / 2f, ScreenSizeY / 2f);
        Matrix scale = Matrix.CreateScale(ScaleFactor, ScaleFactor, 1f);
        Matrix translation = Matrix.CreateTranslation(
            screenCenter.X - entityCenter.X * ScaleFactor,
            screenCenter.Y - entityCenter.Y * ScaleFactor,
        0);

        Transform = Matrix.Multiply(scale, translation);
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        int currentScrollValue = mouseState.ScrollWheelValue;

        if (currentScrollValue != previousScrollValue)
        {
            float delta = (currentScrollValue - previousScrollValue) / 120;
            ScaleFactor += delta * 0.1f;
            ScaleFactor = MathHelper.Clamp(ScaleFactor, 0.1f, 100f);

            previousScrollValue = currentScrollValue;
        }
    }
}


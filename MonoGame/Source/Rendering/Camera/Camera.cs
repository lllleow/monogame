using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Rendering.Camera;

public class Camera
{
    private readonly float followSpeed = 7f;
    private readonly int previousScrollValue;
    private readonly float scaleFactor = 3f;

    public Camera(int screenSizeX, int screenSizeY)
    {
        ScreenSizeX = screenSizeX;
        ScreenSizeY = screenSizeY;
        previousScrollValue = Mouse.GetState().ScrollWheelValue;
    }

    public Matrix Transform { get; set; } = Matrix.Identity;

    private float ScreenSizeX { get; }
    private float ScreenSizeY { get; }

    public void Follow(Vector2 position)
    {
        GameTime gameTime = Globals.GameTime;
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var entityCenter = new Vector2(position.X + (SharedGlobals.PixelSizeX / 2), position.Y + (SharedGlobals.PixelSizeY / 2));
        var screenCenter = new Vector2(ScreenSizeX / 2f, ScreenSizeY / 2f);
        var targetTranslation = Matrix.CreateTranslation(
            screenCenter.X - (entityCenter.X * scaleFactor),
            screenCenter.Y - (entityCenter.Y * scaleFactor),
            0
        );

        var targetTransform = Matrix.Multiply(Matrix.CreateScale(scaleFactor, scaleFactor, 1f), targetTranslation);
        Transform = Matrix.Lerp(Transform, targetTransform, followSpeed * deltaTime);
    }

    public static void Update(GameTime gameTime)
    {
    }
}
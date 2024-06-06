using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Source.Rendering.Camera;

public class Camera
{

    public Matrix Transform { get; set; } = Matrix.Identity;

    private float ScreenSizeX { get; set; }
    private float ScreenSizeY { get; set; }
    private float scaleFactor = 3f;
    private float followSpeed = 7f;
    private int previousScrollValue;

    public Camera(int screenSizeX, int screenSizeY)
    {
        ScreenSizeX = screenSizeX;
        ScreenSizeY = screenSizeY;
        previousScrollValue = Mouse.GetState().ScrollWheelValue;
    }

    public void Follow(IGameEntity entity, GameTime gameTime)
    {
        if (entity == null) return;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 entityCenter = new Vector2(entity.Position.X + Tile.PixelSizeX / 2, entity.Position.Y + Tile.PixelSizeY / 2);
        Vector2 screenCenter = new Vector2(ScreenSizeX / 2f, ScreenSizeY / 2f);

        Matrix targetTranslation = Matrix.CreateTranslation(
            screenCenter.X - entityCenter.X * scaleFactor,
            screenCenter.Y - entityCenter.Y * scaleFactor,
            0);

        Matrix targetTransform = Matrix.Multiply(Matrix.CreateScale(scaleFactor, scaleFactor, 1f), targetTranslation);

        Transform = Matrix.Lerp(Transform, targetTransform, followSpeed * deltaTime);
    }

    public void Update(GameTime gameTime)
    {
    }
}


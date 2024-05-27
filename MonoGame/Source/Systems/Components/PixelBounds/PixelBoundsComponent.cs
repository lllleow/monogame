using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components;
using MonoGame.Source.Systems.Components.Collision;

namespace MonoGame;

public class PixelBoundsComponent : EntityComponent
{
    public bool[,] Mask;
    public Rectangle TextureRectangle;
    public string SpriteSheet;
    public SpriteRendererComponent spriteRendererComponent;

    public override void Initialize()
    {
        if (!Entity.ContainsComponent<SpriteRendererComponent>() && !Entity.GetFirstComponent<SpriteRendererComponent>().Initialized)
        {
            throw new Exception("PixelBoundsComponent requires a SpriteRendererComponent to be present on the entity.");
        }
        else if (!Entity.GetFirstComponent<SpriteRendererComponent>().Initialized)
        {
            throw new Exception("PixelBoundsComponent must be initialized AFTER SpriteRendererComponent. Change the order of AddComponent.");
        }

        spriteRendererComponent = Entity.GetFirstComponent<SpriteRendererComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        SpriteRendererComponent spriteRenderer = Entity.GetFirstComponent<SpriteRendererComponent>();
        Rectangle? currentRectangle = spriteRenderer.TextureRectangle;
        string currentSpritesheetName = null;

        if (Entity.ContainsComponent<CollisionComponent>())
        {
            CollisionComponent collisionComponent = Entity.GetFirstComponent<CollisionComponent>();
            if (collisionComponent.Mode == CollisionMode.CollisionMask)
            {
                currentSpritesheetName = collisionComponent.CollisionMaskSpritesheet;
            }
        }
        else
        {
            currentSpritesheetName = spriteRenderer.SpriteSheet;
        }

        if (spriteRenderer.SpriteSheet != null && spriteRenderer.TextureRectangle != Rectangle.Empty)
        {
            if (currentSpritesheetName == null || currentRectangle == null || currentSpritesheetName != SpriteSheet || currentRectangle.Value != TextureRectangle)
            {
                TextureRectangle = currentRectangle.Value;
                SpriteSheet = currentSpritesheetName;

                Mask = CollisionMaskHandler.GetMaskForTexture(SpriteSheet, TextureRectangle);
            }
        }
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y, (int)spriteRendererComponent.Size.X, (int)spriteRendererComponent.Size.Y);
    }

    PrimitiveBatch primitiveBatch = new PrimitiveBatch(Globals.graphicsDevice.GraphicsDevice);
    public override void Draw(SpriteBatch spriteBatch)
    {
        primitiveBatch.Begin(PrimitiveType.LineList);
        Rectangle drawingArea = GetRectangle();

        for (int x = 0; x < Mask.GetLength(0); x++)
        {
            for (int y = 0; y < Mask.GetLength(1); y++)
            {
                if (Mask[x, y])
                {
                    Vector2 topLeft = new Vector2(drawingArea.X + x, drawingArea.Y + y);
                    Vector2 topRight = new Vector2(topLeft.X + 1, topLeft.Y);
                    Vector2 bottomLeft = new Vector2(topLeft.X, topLeft.Y + 1);
                    Vector2 bottomRight = new Vector2(topRight.X, bottomLeft.Y);

                    primitiveBatch.AddVertex(topLeft, Color.Red);
                    primitiveBatch.AddVertex(topRight, Color.Red);

                    primitiveBatch.AddVertex(topRight, Color.Red);
                    primitiveBatch.AddVertex(bottomRight, Color.Red);

                    primitiveBatch.AddVertex(bottomRight, Color.Red);
                    primitiveBatch.AddVertex(bottomLeft, Color.Red);

                    primitiveBatch.AddVertex(bottomLeft, Color.Red);
                    primitiveBatch.AddVertex(topLeft, Color.Red);
                }
            }
        }

        primitiveBatch.End();
    }
}

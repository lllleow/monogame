using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Components.SpriteRenderer;

namespace MonoGame.Source.Systems.Components.PixelBounds;

public class PixelBoundsComponent : EntityComponent
{
    public bool[,] Mask;
    public Rectangle TextureRectangle;
    public string SpriteSheet;
    public SpriteRendererComponent SpriteRendererComponent;
    public static bool ShowEntityPixelPerfectCollision = false;

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

        SpriteRendererComponent = Entity.GetFirstComponent<SpriteRendererComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        var spriteRenderer = Entity.GetFirstComponent<SpriteRendererComponent>();
        Rectangle? currentRectangle = spriteRenderer.TextureRectangle;
        string currentSpritesheetName = null;

        if (Entity.ContainsComponent<CollisionComponent>())
        {
            var collisionComponent = Entity.GetFirstComponent<CollisionComponent>();
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
        return new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y, (int)SpriteRendererComponent.Size.X, (int)SpriteRendererComponent.Size.Y);
    }

    private readonly PrimitiveBatch primitiveBatch = new(Globals.GraphicsDevice.GraphicsDevice);
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (ShowEntityPixelPerfectCollision)
        {
            primitiveBatch.Begin(PrimitiveType.LineList);
            var drawingArea = GetRectangle();

            for (var x = 0; x < Mask.GetLength(0); x++)
            {
                for (var y = 0; y < Mask.GetLength(1); y++)
                {
                    if (Mask[x, y])
                    {
                        var topLeft = new Vector2(drawingArea.X + x, drawingArea.Y + y);
                        var topRight = new Vector2(topLeft.X + 1, topLeft.Y);
                        var bottomLeft = new Vector2(topLeft.X, topLeft.Y + 1);
                        var bottomRight = new Vector2(topRight.X, bottomLeft.Y);

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
}

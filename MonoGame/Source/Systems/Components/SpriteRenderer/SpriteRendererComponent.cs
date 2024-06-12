using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Common;
using MonoGame_Common.Util.Helpers;
using MonoGame.Source.Utils.Helpers;
using MonoGame.Source.Utils.Loaders;

namespace MonoGame.Source.Systems.Components.SpriteRenderer;

public class SpriteRendererComponent : EntityComponent
{
    public SpriteRendererComponent()
    {
    }

    public SpriteRendererComponent(string spritesheet, Vector2 textureCoordinates, Vector2 textureSize)
    {
        SpriteSheet = spritesheet;
        TextureRectangle = new Rectangle(
            (int)textureCoordinates.X,
            (int)textureCoordinates.Y,
            (int)textureSize.X,
            (int)textureSize.Y);
    }

    public string SpriteSheet { get; set; }

    public Rectangle TextureRectangle { get; set; } = Rectangle.Empty;

    public Vector2 Scale { get; set; } = new(1, 1);

    public Vector2 Size { get; set; } = new(SharedGlobals.PixelSizeX, SharedGlobals.PixelSizeY);

    public void UpdateTexture(string spritesheet, Rectangle textureRectangle)
    {
        SpriteSheet = spritesheet;
        TextureRectangle = textureRectangle;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SpriteSheet), Entity.Position, TextureRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0.5f);
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y, (int)Size.X, (int)Size.Y);
    }

    public TextureLocation GetTextureLocation()
    {
        return new TextureLocation(SpriteSheet, RectangleHelper.ConvertToDrawingRectangle(TextureRectangle));
    }
}
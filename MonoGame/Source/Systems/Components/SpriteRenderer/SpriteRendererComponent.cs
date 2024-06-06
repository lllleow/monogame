using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Components;
using MonoGame.Source.Util.Loaders;

namespace MonoGame;

public class SpriteRendererComponent : EntityComponent
{

    public string SpriteSheet;

    public Rectangle TextureRectangle = Rectangle.Empty;

    public Vector2 Scale = new Vector2(1, 1);

    public Vector2 Size = new Vector2(Tile.PixelSizeX, Tile.PixelSizeY);

    public SpriteRendererComponent()
    {
    }

    public SpriteRendererComponent(string spritesheet, Vector2 textureCoordinates, Vector2 textureSize)
    {
        SpriteSheet = spritesheet;
        TextureRectangle = new Rectangle((int)textureCoordinates.X, (int)textureCoordinates.Y, (int)textureSize.X, (int)textureSize.Y);
    }

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
        return new TextureLocation(SpriteSheet, TextureRectangle);
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components;
using MonoGame.Source.Util.Loaders;

namespace MonoGame;

public class SpriteRendererComponent : EntityComponent
{
    public string SpriteSheet;
    public Rectangle TextureRectangle;
    public Vector2 Scale;

    public override void Initialize()
    {
        TextureRectangle = Rectangle.Empty;
        Scale = new Vector2(1, 1);
    }

    public void UpdateTexture(string spritesheet, Rectangle textureRectangle)
    {
        SpriteSheet = spritesheet;
        TextureRectangle = textureRectangle;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SpriteSheet), Entity.Position, TextureRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
    }
}

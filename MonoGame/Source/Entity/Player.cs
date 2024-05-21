using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class Player : DrawablePhysicalEntity
{
    public Player(String spriteSheet, Vector2 position, Vector2 size)
    {
        SpritesheetName = "textures/player_spritesheet";
        TextureX = 0;
        TextureY = 0;
        Position = position;
        Size = size;
        Speed = new Vector2(5, 5);
    }

    public Rectangle GetCurrentSpriteRectangle()
    {
        return new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SpritesheetName), Position, GetCurrentSpriteRectangle(), Color.White);
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();


        if (state.IsKeyDown(Keys.W))
        {
            this.Position = new Vector2(this.Position.X, this.Position.Y - Speed.Y);
        }
        if (state.IsKeyDown(Keys.A))
        {
            this.Position = new Vector2(this.Position.X - Speed.X, this.Position.Y);
        }
        if (state.IsKeyDown(Keys.S))
        {
            this.Position = new Vector2(this.Position.X, this.Position.Y + Speed.Y);
        }
        if (state.IsKeyDown(Keys.D))
        {
            this.Position = new Vector2(this.Position.X + Speed.X, this.Position.Y);
        }
    }
}

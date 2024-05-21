using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class Player : DrawablePhysicalEntity
{
    public Player(String texture, Vector2 position, Vector2 size)
    {
        Texture = Globals.contentManager.Load<Texture2D>(texture);
        Position = position;
        Size = size;
        Speed = new Vector2(5, 5);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
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

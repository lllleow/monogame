using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class Player : DrawablePhysicalEntity
{
    public Player(String spriteSheet, Vector2 position, int sizeX, int sizeY)
    {
        SpritesheetName = "textures/player_spritesheet";
        TextureX = 0;
        TextureY = 0;
        Position = position;
        PixelSizeX = sizeX;
        PixelSizeY = sizeY;
        Speed = new Vector2(5, 5);
        AddComponent(new AnimatorComponent(this, AnimationRegistry.GetAnimation("player_movement_animation")));
    }

    public void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.W))
        {
            GetFirstComponent<AnimatorComponent>().SetState("walking_back");
            this.Position = new Vector2(this.Position.X, this.Position.Y - Speed.Y);
        }
        if (state.IsKeyDown(Keys.A))
        {
            GetFirstComponent<AnimatorComponent>().SetState("walking_left");
            this.Position = new Vector2(this.Position.X - Speed.X, this.Position.Y);
        }
        if (state.IsKeyDown(Keys.S))
        {
            GetFirstComponent<AnimatorComponent>().SetState("walking_front");
            this.Position = new Vector2(this.Position.X, this.Position.Y + Speed.Y);
        }
        if (state.IsKeyDown(Keys.D))
        {
            GetFirstComponent<AnimatorComponent>().SetState("walking_right");
            this.Position = new Vector2(this.Position.X + Speed.X, this.Position.Y);
        }
    }
}

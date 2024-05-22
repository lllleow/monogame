using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame;

public class Player : GameEntity
{
    public Player(Vector2 position, int sizeX, int sizeY)
    {
        Position = position;
        Speed = new Vector2(2, 2);
        AddComponent(new AnimatorComponent(this, AnimationBundleRegistry.GetAnimationBundle("base.player")));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.W))
        {
            GetFirstComponent<AnimatorComponent>().PlayAnimation("walking_back");
            this.Position = new Vector2(this.Position.X, this.Position.Y - Speed.Y);
        }
        if (state.IsKeyDown(Keys.A))
        {
            GetFirstComponent<AnimatorComponent>().PlayAnimation("walking_left");
            this.Position = new Vector2(this.Position.X - Speed.X, this.Position.Y);
        }
        if (state.IsKeyDown(Keys.S))
        {
            GetFirstComponent<AnimatorComponent>().PlayAnimation("walking_front");
            this.Position = new Vector2(this.Position.X, this.Position.Y + Speed.Y);
        }
        if (state.IsKeyDown(Keys.D))
        {
            GetFirstComponent<AnimatorComponent>().PlayAnimation("walking_right");
            this.Position = new Vector2(this.Position.X + Speed.X, this.Position.Y);
        }

        if (state.IsKeyUp(Keys.W) && state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.S) && state.IsKeyUp(Keys.D))
        {
            GetFirstComponent<AnimatorComponent>().PlayAnimation("idle");
        }
    }
}

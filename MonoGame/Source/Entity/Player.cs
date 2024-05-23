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
        Speed = new Vector2(5, 5);
        AddComponent(new AnimatorComponent(this, AnimationBundleRegistry.GetAnimationBundle("base.player")));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.W))
        {
            if (Move(Direction.TOP, Speed))
            {
                GetFirstComponent<AnimatorComponent>().PlayAnimation("walking_back");
            }
        }
        if (state.IsKeyDown(Keys.A))
        {
            if (Move(Direction.LEFT, Speed))
            {
                GetFirstComponent<AnimatorComponent>().PlayAnimation("walking_left");
            }
        }
        if (state.IsKeyDown(Keys.S))
        {
            if (Move(Direction.BOTTOM, Speed))
            {
                GetFirstComponent<AnimatorComponent>().PlayAnimation("walking_front");
            }
        }
        if (state.IsKeyDown(Keys.D))
        {
            if (Move(Direction.RIGHT, Speed))
            {
                GetFirstComponent<AnimatorComponent>().PlayAnimation("walking_right");
            }
        }

        if (state.IsKeyUp(Keys.W) && state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.S) && state.IsKeyUp(Keys.D))
        {
            GetFirstComponent<AnimatorComponent>().PlayAnimation("idle");
        }
    }
}

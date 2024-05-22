using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class AnimatorComponent : IEntityComponent
{
    int CurrentTime;
    public IGameEntity Entity { get; set; }
    public IAnimation Animation;

    public AnimatorComponent(IGameEntity entity, IAnimation animation)
    {
        Entity = entity;
        Animation = animation;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        double percentage = CurrentTime / Animation.Duration;
        Rectangle spriteRectangle = Animation.GetSpriteRectangle(percentage);
        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(Animation.SpriteSheet), Entity.Position, spriteRectangle, Color.White);
    }

    public void Initialize()
    {
        string stateCopy = Animation.CurrentState;
        if (stateCopy == null)
        {
            stateCopy = Animation.AnimationStates.Keys.ToList().First();
        }
        Animation.CurrentState = stateCopy;
    }

    public void SetState(string state)
    {
        Animation.SetState(state);
    }

    public void Update(GameTime gameTime)
    {
        if (Entity is IDrawable)
        {
            CurrentTime++;
            if (CurrentTime > Animation.Duration)
            {
                CurrentTime = 0;
            }
        }
    }
}

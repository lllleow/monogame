using System;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Animation;

namespace MonoGame;

public interface IAnimationState
{
    public Animation Animation { get; set; }
    public IAnimationBundle AnimationBundle { get; set; }
    public Action<IAnimationState> OnStateEnded { get; set; }
    public int CurrentTime { get; set; }
    public bool FinishedPlayingAnimation { get; set; }
    public bool StateEnded { get; set; }
    public abstract void Update(GameTime gameTime);
    public abstract void Start();
    public abstract (int TextureX, int TextureY) GetTextureCoordinates();
    public abstract Rectangle GetTextureRectangle();
    public abstract double GetAnimationPercentage();
}

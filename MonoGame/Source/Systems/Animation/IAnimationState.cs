using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Source.Systems.Animation;

public interface IAnimationState
{
    public Animation Animation { get; set; }
    public IAnimationBundle AnimationBundle { get; set; }
    public Action<IAnimationState> OnStateEnded { get; set; }
    public int CurrentTime { get; set; }
    public bool FinishedPlayingAnimation { get; set; }
    public bool StateEnded { get; set; }
    public void Update(GameTime gameTime);
    public void Start();
    public (int TextureX, int TextureY) GetTextureCoordinates();
    public Rectangle GetTextureRectangle();
    public double GetAnimationPercentage();
}
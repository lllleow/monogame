using System;
using MonoGame.Source.Systems.Animation;
public class PlayerMovementAnimationBundle : AnimationBundle
{
    public PlayerMovementAnimationBundle()
    {
        Id = "base.player";
        SpriteSheet = "textures/player_sprite_2";

        AnimationTransitions.Add(new ChangeStateOnEndAnimationTransition("walking_front", "idle"));
        AnimationTransitions.Add(new ChangeStateOnEndAnimationTransition("walking_back", "idle"));
        AnimationTransitions.Add(new ChangeStateOnEndAnimationTransition("walking_right", "idle"));
        AnimationTransitions.Add(new ChangeStateOnEndAnimationTransition("walking_left", "idle"));

        CreateAnimation(new Animation("walking_front", false, 0, 50, 3));
        CreateAnimation(new Animation("walking_back", false, 1, 50, 3));
        CreateAnimation(new Animation("walking_right", false, 2, 50, 3));
        CreateAnimation(new Animation("walking_left", false, 3, 50, 3));
        CreateAnimation(new Animation("idle", true, 4, 200, 3));
    }
}

return new PlayerMovementAnimationBundle();

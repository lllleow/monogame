using System;
using MonoGame.Source.Systems.Animation;
public class PlayerMovementAnimationBundle : AnimationBundle
{
    public PlayerMovementAnimationBundle()
    {
        Id = "base.player";
        SpriteSheet = "textures/player_sprite_2";
        CollisionMaskSpritesheet = "textures/player_collision_mask";

        // AddTransition(new ChangeStateOnEndAnimationTransition("walking_front", "idle"));
        // AddTransition(new ChangeStateOnEndAnimationTransition("walking_back", "idle"));
        // AddTransition(new ChangeStateOnEndAnimationTransition("walking_right", "idle"));
        // AddTransition(new ChangeStateOnEndAnimationTransition("walking_left", "idle"));

        CreateAnimation(new Animation("walking_front", false, 0, 25, 3));
        CreateAnimation(new Animation("walking_back", false, 1, 25, 3));
        CreateAnimation(new Animation("walking_right", false, 2, 25, 3));
        CreateAnimation(new Animation("walking_left", false, 3, 25, 3));
        CreateAnimation(new Animation("idle", true, 4, 200, 3, true));
    }
}

return new PlayerMovementAnimationBundle();

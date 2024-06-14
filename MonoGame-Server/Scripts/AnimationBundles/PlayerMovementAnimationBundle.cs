using System;
using MonoGame_Common.Systems.Animation;
public class PlayerMovementAnimationBundle : AnimationBundle
{
    public PlayerMovementAnimationBundle()
    {
        Id = "base.player";
        SpriteSheet = "textures/player_sprite_2";
        CollisionMaskSpritesheet = "textures/player_sprite_2_collision_mask";

        CreateAnimation(new Animation("walking_front", false, 0, 25, 3));
        CreateAnimation(new Animation("walking_back", false, 1, 25, 3));
        CreateAnimation(new Animation("walking_right", false, 2, 25, 3));
        CreateAnimation(new Animation("walking_left", false, 3, 25, 3));
        CreateAnimation(new Animation("idle", true, 4, 200, 3, true));
    }
}

return new PlayerMovementAnimationBundle();

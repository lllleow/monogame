using System;

public class PlayerMovementAnimationBundle : AnimationBundle
{
    public PlayerMovementAnimationBundle()
    {
        Id = "base.player";
        SpriteSheet = "textures/player_sprite_2";

        CreateAnimation(new Animation("walking_front", 0, 50, 3));
        CreateAnimation(new Animation("walking_back", 1, 50, 3));
        CreateAnimation(new Animation("walking_right", 2, 50, 3));
        CreateAnimation(new Animation("walking_left", 3, 50, 3));
        CreateAnimation(new Animation("idle", 4, 200, 3));
    }
}

return new PlayerMovementAnimationBundle();

using System;

public class PlayerMovementAnimation : Animation
{
    public PlayerMovementAnimation()
    {
        Id = "player_movement_animation";
        SpriteSheet = "textures/player_spritesheet";

        Duration = 10;

        RegisterRowForState("walking_front", 0, 4);
        RegisterRowForState("walking_back", 1, 4);
        RegisterRowForState("walking_right", 2, 4);
        RegisterRowForState("walking_left", 3, 4);
    }
}

return new PlayerMovementAnimation();

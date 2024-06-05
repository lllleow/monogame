using Microsoft.Xna.Framework.Graphics;

public class FenceTile : Tile
{
    public FenceTile()
    {
        Id = "base.wall";
        Name = "Wall";
        SpritesheetName = "textures/wall_spritesheet";
        CollisionMaskSpritesheetName = "textures/wall_spritesheet_collision_mask";
        CollisionMode = CollisionMode.CollisionMask;
        TextureProcessor = SimpleConnectionTileTextureProcessor.instance;
        Walkable = false;
    }
}

return new FenceTile();

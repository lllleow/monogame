using Microsoft.Xna.Framework.Graphics;

public class FenceTile : Tile
{
    public FenceTile()
    {
        Id = "base.fence";
        Name = "Fence";
        SpritesheetName = "textures/fence_spritesheet";
        CollisionMaskSpritesheetName = "textures/fence_spritesheet_collision_mask";
        CollisionMode = CollisionMode.CollisionMask;
        TextureProcessor = SimpleConnectionTileTextureProcessor.instance;
        ConnectableTiles = ["base.grass", "base.stone"];
        PixelOffsetY = 8;
        PixelOffsetX = 8;
        Walkable = false;
    }
}

return new FenceTile();

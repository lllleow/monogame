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
        TextureType = TileTextureType.SimpleConnecting;
        ConnectableTiles = ["base.grass"];
        Walkable = false;
    }
}

return new FenceTile();

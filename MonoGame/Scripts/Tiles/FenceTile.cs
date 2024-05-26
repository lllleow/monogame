using Microsoft.Xna.Framework.Graphics;

public class FenceTile : Tile
{
    public FenceTile()
    {
        Id = "base.fence";
        Name = "Fence";
        SpritesheetName = "textures/fence_spritesheet";
        TextureX = 1;
        TextureY = 1;
        TextureType = TileTextureType.SimpleConnecting;
        Walkable = false;
    }
}

return new FenceTile();

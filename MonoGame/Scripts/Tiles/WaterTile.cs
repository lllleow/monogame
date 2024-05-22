using System;

public class WaterTile : Tile
{
    public WaterTile()
    {
        Id = "base.water";
        Name = "Water";
        SpritesheetName = "textures/spritesheet";
        TextureX = 2;
        TextureY = 0;
    }
}

return new WaterTile();
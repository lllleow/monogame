using System;
using System.Collections.Generic;

public class WaterTile : Tile
{
    public WaterTile()
    {
        Id = "base.water";
        Name = "Water";
        SpritesheetName = "textures/spritesheet";
        TextureX = 0;
        TextureY = 0;
    }
}

return new WaterTile();
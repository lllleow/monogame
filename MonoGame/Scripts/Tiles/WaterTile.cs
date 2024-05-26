using System;
using System.Collections.Generic;

public class WaterTile : Tile
{
    public WaterTile()
    {
        Id = "base.water";
        Name = "Water";
        SpritesheetName = "textures/spritesheet";
        TextureX = 2;
        TextureY = 0;
        DoubleTextureSize = true;
        CollisionCriteria = new List<TileCollisionCriteria> { };
    }
}

return new WaterTile();
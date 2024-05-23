using System;

public class StoneTile : Tile
{
    public StoneTile()
    {
        Id = "base.stone";
        Name = "Stone";
        SpritesheetName = "textures/spritesheet";
        TextureX = 1;
        TextureY = 0;
        DoubleTextureSize = true;
    }
}

return new StoneTile();
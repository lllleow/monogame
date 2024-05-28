using Microsoft.Xna.Framework.Graphics;

public class StoneTile : Tile
{
    public StoneTile()
    {
        Id = "base.stone";
        Name = "Stone";
        SpritesheetName = "textures/stone_spritesheet";
        TextureProcessor = ComplexConnectionTileTextureProcessor.standard;
        Walkable = false;
    }
}

return new StoneTile();

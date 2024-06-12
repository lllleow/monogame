using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Tiles;
using MonoGame_Common.Util.Tile.TextureProcessors;
using MonoGame_Common.Enums;

public class StoneTile : Tile
{
    public StoneTile()
    {
        Id = "base.stone";
        Name = "Stone";
        SpritesheetName = "textures/stone_spritesheet";
        TextureProcessor = ComplexConnectionTileTextureProcessor.Instance;
        Walkable = false;
    }
}

return new StoneTile();

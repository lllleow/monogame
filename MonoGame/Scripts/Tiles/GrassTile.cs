using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Tile.TextureProcessors;
using MonoGame_Common.Enums;

public class GrassTile : CommonTile
{
    public GrassTile()
    {
        Id = "base.grass";
        Name = "Grass";
        SpritesheetName = "textures/grass_spritesheet";
        TextureProcessor = ComplexConnectionTileTextureProcessor.Instance;
        Walkable = false;
    }
}

return new GrassTile();

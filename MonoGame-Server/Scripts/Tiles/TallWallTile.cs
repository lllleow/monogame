using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Tile.TextureProcessors;
using MonoGame_Common.Enums;

public class TallWallTIle : CommonTile
{
    public TallWallTIle()
    {
        Id = "base.wall_tall";
        Name = "Tall Wall";
        TileTextureSizeY = 2;
        SpritesheetName = "textures/tall_wall_spritesheet";
        CollisionMode = CollisionMode.BoundingBox;
        TextureProcessor = SimpleConnectionTileTextureProcessor.Instance;
        Walkable = false;
    }
}

return new TallWallTIle();

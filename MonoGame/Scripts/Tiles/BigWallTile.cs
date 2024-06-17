using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Tile.TextureProcessors;
using MonoGame_Common.Enums;

public class BigWallTile : WallTile
{
    public BigWallTile()
    {
        Id = "base.wall_big";
        Name = "Big Wall";
        SpritesheetName = "textures/big_wall_spritesheet";
        CollisionMode = CollisionMode.BoundingBox;
        TextureProcessor = SimpleConnectionTileTextureProcessor.Instance;
        Walkable = false;
    }
}

return new BigWallTile();

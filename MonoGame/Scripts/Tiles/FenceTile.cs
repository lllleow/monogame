using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Tiles;
using MonoGame_Common.Util.Tile.TextureProcessors;
using MonoGame_Common.Enums;

public class FenceTile : Tile
{
    public FenceTile()
    {
        Id = "base.fence";
        Name = "Fence";
        SpritesheetName = "textures/fence_spritesheet";
        CollisionMaskSpritesheetName = "textures/fence_spritesheet_collision_mask";
        CollisionMode = CollisionMode.CollisionMask;
        TextureProcessor = SimpleConnectionTileTextureProcessor.Instance;
        ConnectableTiles = ["base.grass", "base.stone"];
        Walkable = false;
    }
}

return new FenceTile();

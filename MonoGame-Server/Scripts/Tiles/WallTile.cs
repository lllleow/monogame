﻿using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Tile.TextureProcessors;
using MonoGame_Common.Enums;

public class FenceTile : CommonTile
{
    public FenceTile()
    {
        Id = "base.wall";
        Name = "Wall";
        SpritesheetName = "textures/wall_spritesheet";
        CollisionMaskSpritesheetName = "textures/wall_spritesheet_collision_mask";
        CollisionMode = CollisionMode.CollisionMask;
        TextureProcessor = SimpleConnectionTileTextureProcessor.Instance;
        Walkable = false;
    }
}

return new FenceTile();

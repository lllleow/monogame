﻿using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Systems.Tiles.TextureProcessors;
using MonoGame_Common.Enums;

public class FenceTile : ITile
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

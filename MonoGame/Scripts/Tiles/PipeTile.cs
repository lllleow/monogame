using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Systems.Tiles.TextureProcessors;
using MonoGame_Common.Enums;

public class PipeTile : Tile
{
    public PipeTile()
    {
        Id = "base.pipe";
        Name = "Pipe";
        SpritesheetName = "textures/pipe_spritesheet";
        TextureProcessor = SimpleConnectionTileTextureProcessor.Instance;
    }
}

return new PipeTile();

using System.Collections.Generic;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Tile.TextureProcessors;
using MonoGame_Common.Enums;

public class PipeTile : CommonTile
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

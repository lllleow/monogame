using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class PipeTile : Tile
{
    public PipeTile()
    {
        Id = "base.pipe";
        Name = "Pipe";
        SpritesheetName = "textures/pipe_spritesheet";
        TextureProcessor = SimpleConnectionTileTextureProcessor.standard;
    }
}

return new PipeTile();

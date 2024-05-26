using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class PipeTile : Tile
{
    public PipeTile()
    {
        Id = "base.pipe";
        Name = "Pipe";
        SpritesheetName = "textures/pipe_spritesheet";
        TextureType = TileTextureType.SimpleConnecting;
    }
}

return new PipeTile();

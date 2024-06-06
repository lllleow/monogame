using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Systems.Tiles.TextureProcessors;

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

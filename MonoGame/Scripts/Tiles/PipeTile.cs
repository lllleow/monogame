using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class PipeTile : Tile
{
    public PipeTile()
    {
        Id = "base.pipe";
        Name = "Pipe";
        SpritesheetName = "textures/pipe_spritesheet";
        TextureX = 1;
        TextureY = 1;
        TextureType = TileTextureType.SimpleConnecting;
        CollisionCriteria = new List<TileCollisionCriteria> { TileCollisionCriteria.PassableLeft, TileCollisionCriteria.PassableRight, TileCollisionCriteria.PassableTop };
    }
}

return new PipeTile();

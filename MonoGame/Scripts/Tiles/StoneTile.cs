using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Tiles.TextureProcessors;

public class StoneTile : Tile
{
    public StoneTile()
    {
        Id = "base.stone";
        Name = "Stone";
        SpritesheetName = "textures/stone_spritesheet";
        TextureProcessor = ComplexConnectionTileTextureProcessor.Instance;
        Walkable = false;
    }
}

return new StoneTile();

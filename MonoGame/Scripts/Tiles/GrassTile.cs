using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Systems.Tiles.TextureProcessors;
using MonoGame_Common.Enums;

public class GrassTile : Tile
{
    public GrassTile()
    {
        Id = "base.grass";
        Name = "Grass";
        SpritesheetName = "textures/grass_spritesheet";
        TextureProcessor = ComplexConnectionTileTextureProcessor.Instance;
        Walkable = false;
    }
}

return new GrassTile();

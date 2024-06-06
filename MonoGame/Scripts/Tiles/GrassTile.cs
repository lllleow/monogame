using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Systems.Tiles.TextureProcessors;

public class GrassTile : Tile
{
    public GrassTile()
    {
        Id = "base.grass";
        Name = "Grass";
        SpritesheetName = "textures/grass_spritesheet";
        TextureProcessor = ComplexConnectionTileTextureProcessor.instance;
        Walkable = false;
    }
}

return new GrassTile();

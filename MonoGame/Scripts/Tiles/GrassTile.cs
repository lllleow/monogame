using Microsoft.Xna.Framework.Graphics;

public class GrassTile : Tile
{
    public GrassTile()
    {
        Id = "base.grass";
        Name = "Grass";
        SpritesheetName = "textures/grass_spritesheet";
        TextureX = 1;
        TextureY = 1;
        TextureType = TileTextureType.CompleteConnecting;
    }
}

return new GrassTile();

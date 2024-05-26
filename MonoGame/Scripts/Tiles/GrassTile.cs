using Microsoft.Xna.Framework.Graphics;

public class GrassTile : Tile
{
    public GrassTile()
    {
        Id = "base.grass";
        Name = "Grass";
        SpritesheetName = "textures/grass_spritesheet";
        TextureType = TileTextureType.CompleteConnecting;
        Walkable = false;
    }
}

return new GrassTile();

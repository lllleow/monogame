using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.Utils;

namespace MonoGame;

public class TileSlotComponent : SlotComponent
{
    public ITile Tile;

    public TileSlotComponent(string name, ITile tile, Vector2 localPosition) : base(name, localPosition)
    {
        Tile = tile;
    }

    public void SetTile(ITile tile)
    {
        Tile = tile;
    }

    public override TextureLocation GetDrawable()
    {
        TextureLocation textureLocation = Tile?.GetTextureLocation();
        textureLocation.TextureRectangle = rectangleHelper.GetTextureRectangleFromCoordinates(Tile?.DefaultTextureCoordinates.Item1 ?? 0, Tile?.DefaultTextureCoordinates.Item2 ?? 0);
        return textureLocation;
    }
}

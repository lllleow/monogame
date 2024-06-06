using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

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
        var textureLocation = Tile?.GetTextureLocation();
        textureLocation.TextureRectangle = RectangleHelper.GetTextureRectangleFromCoordinates(Tile?.DefaultTextureCoordinates.Item1 ?? 0, Tile?.DefaultTextureCoordinates.Item2 ?? 0);
        return textureLocation;
    }
}

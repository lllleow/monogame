using Microsoft.Xna.Framework;
using MonoGame.Source.Utils.Helpers;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Helpers;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class TileSlotComponent : SlotComponent
{
    public TileSlotComponent(string name, CommonTile tile, Vector2 localPosition) : base(name, localPosition)
    {
        Tile = tile;
    }

    public CommonTile Tile { get; set; }

    public void SetTile(CommonTile tile)
    {
        Tile = tile;
    }

    public override TextureLocation GetDrawable()
    {
        TextureLocation textureLocation = new TextureLocation(Tile?.SpritesheetName, RectangleHelper.ConvertToDrawingRectangle(RectangleHelper.GetTextureRectangleFromCoordinates(Tile?.DefaultTextureCoordinates.TextureCoordinateX ?? 0, Tile?.DefaultTextureCoordinates.TextureCoordinateY ?? 0)));
        return textureLocation;
    }
}
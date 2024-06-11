using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class TileSlotComponent : SlotComponent
{
    public TileSlotComponent(string name, Tile tile, Vector2 localPosition) : base(name, localPosition)
    {
        Tile = tile;
    }

    public Tile Tile { get; set; }

    public void SetTile(Tile tile)
    {
        Tile = tile;
    }

    public override TextureLocation GetDrawable()
    {
        TextureLocation textureLocation = new TextureLocation(Tile?.SpritesheetName, RectangleHelper.GetTextureRectangleFromCoordinates(Tile?.DefaultTextureCoordinates.TextureCoordinateX ?? 0, Tile?.DefaultTextureCoordinates.TextureCoordinateY ?? 0));
        return textureLocation;
    }
}
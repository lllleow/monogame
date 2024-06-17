using System.Drawing;
using LiteNetLib.Utils;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Helpers;
using MonoGame_Common.Util.Tile;

namespace MonoGame_Common.States.TileComponents;

public class TextureRendererTileComponentState : TileComponentState
{
    public int TextureX { get; set; }
    public int TextureY { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        TextureX = reader.GetInt();
        TextureY = reader.GetInt();
    }

    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(TextureX);
        writer.Put(TextureY);
    }

    public TextureLocation GetTextureLocation()
    {
        return new TextureLocation(TileState?.GetCommonTile()?.SpritesheetName, GetSpriteRectangle());
    }

    public Rectangle GetSpriteRectangle()
    {
        CommonTile? commonTile = TileState?.GetCommonTile();
        if (commonTile == null) return new Rectangle(0, 0, SharedGlobals.PixelSizeY, SharedGlobals.PixelSizeY);
        return new Rectangle(TextureX * SharedGlobals.PixelSizeX, TextureY * SharedGlobals.PixelSizeY, commonTile.TileTextureSizeY * SharedGlobals.PixelSizeX, commonTile.TileTextureSizeY * SharedGlobals.PixelSizeY);
    }

    public void UpdateTextureCoordinates(TileNeighborConfiguration configuration)
    {
        (int, int) coordinates = TileState?.GetCommonTile()?.TextureProcessor?.Process(configuration) ?? (0, 0);
        TextureX = coordinates.Item1;
        TextureY = coordinates.Item2;
    }
}

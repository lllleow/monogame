using LiteNetLib.Utils;
using MonoGame_Common.Systems.Tiles.Interfaces;

namespace MonoGame_Common.Util.Tile.TileComponents;

public interface ITileComponent : INetSerializable
{
    public CommonTile Tile { get; set; }
}

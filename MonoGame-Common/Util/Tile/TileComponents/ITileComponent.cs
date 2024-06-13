using LiteNetLib.Utils;
using MonoGame_Common.States.TileComponents;
using MonoGame_Common.Systems.Tiles.Interfaces;

namespace MonoGame_Common.Util.Tile.TileComponents;

public interface ITileComponent
{
    public CommonTile Tile { get; set; }
    public TileComponentState GetTileComponentState();
}

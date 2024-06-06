using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame;

public class PlaceTileNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        PlaceTileNetworkMessage placeTileNetworkMessage = (PlaceTileNetworkMessage)message;
        Globals.World.SetTileAtPosition(placeTileNetworkMessage.TileId, TileDrawLayer.Tiles, placeTileNetworkMessage.PosX, placeTileNetworkMessage.PosY);
    }
}

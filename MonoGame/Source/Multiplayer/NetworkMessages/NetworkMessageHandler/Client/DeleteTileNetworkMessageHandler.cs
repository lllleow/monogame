using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame;

public class DeleteTileNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        DeleteTileNetworkMessage deleteTileNetworkMessage = (DeleteTileNetworkMessage)message;
        Globals.World.DeleteTile(TileDrawLayer.Tiles, deleteTileNetworkMessage.PosX, deleteTileNetworkMessage.PosY);
    }
}

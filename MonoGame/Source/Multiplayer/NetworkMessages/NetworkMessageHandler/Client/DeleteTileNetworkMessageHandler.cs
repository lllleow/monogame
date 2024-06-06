using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;
using MonoGame.Source.Rendering.Enum;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessageHandler.Client;

public class DeleteTileNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        var deleteTileNetworkMessage = (DeleteTileNetworkMessage)message;
        Globals.World.DeleteTile(TileDrawLayer.Tiles, deleteTileNetworkMessage.PosX, deleteTileNetworkMessage.PosY);
    }
}

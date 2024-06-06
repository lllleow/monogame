using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;
using MonoGame.Source.Rendering.Enum;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessageHandler.Client;

public class PlaceTileNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        var placeTileNetworkMessage = (PlaceTileNetworkMessage)message;
        _ = Globals.World.SetTileAtPosition(placeTileNetworkMessage.TileId, TileDrawLayer.Tiles, placeTileNetworkMessage.PosX, placeTileNetworkMessage.PosY);
    }
}

using LiteNetLib;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;

namespace MonoGame_Server.Systems.Server.MessageHandlers;

public class RequestToPlaceTileNetworkMessageHandler : IServerMessageHandler
{
    public void Execute(NetPeer peer, byte channel, INetworkMessage message)
    {
        var requestToPlaceTileNetworkMessage = (RequestToPlaceTileNetworkMessage)message;
        var tile = NetworkServer.Instance.ServerWorld.GetTileAtPosition(requestToPlaceTileNetworkMessage.Layer, requestToPlaceTileNetworkMessage.PosX, requestToPlaceTileNetworkMessage.PosY);
        if (tile == null)
        {
            NetworkServer.Instance.ServerWorld.SetTileAtPosition(requestToPlaceTileNetworkMessage.TileId, requestToPlaceTileNetworkMessage.Layer, requestToPlaceTileNetworkMessage.PosX, requestToPlaceTileNetworkMessage.PosY);
        }
        else
        {
            NetworkServer.Instance.ServerWorld.DestroyTileAtPosition(requestToPlaceTileNetworkMessage.Layer, requestToPlaceTileNetworkMessage.PosX, requestToPlaceTileNetworkMessage.PosY);
        }
    }

    public bool Validate(NetPeer peer, byte channel, INetworkMessage message)
    {
        return true;
    }
}

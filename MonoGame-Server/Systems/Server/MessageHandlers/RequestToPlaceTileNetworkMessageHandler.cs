using LiteNetLib;
using MonoGame;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame_Server.Systems.Server;
using MonoGame_Server.Systems.Server.MessageHandlers;

namespace MonoGame_Server;

public class RequestToPlaceTileNetworkMessageHandler : IServerMessageHandler
{
    public void Execute(NetPeer peer, byte channel, INetworkMessage message)
    {
        RequestToPlaceTileNetworkMessage requestToPlaceTileNetworkMessage = (RequestToPlaceTileNetworkMessage)message;

        NetworkServer.Instance.ServerWorld.SetTileAtPosition(requestToPlaceTileNetworkMessage.TileId, requestToPlaceTileNetworkMessage.Layer, requestToPlaceTileNetworkMessage.PosX, requestToPlaceTileNetworkMessage.PosY);
    }

    public bool Validate(NetPeer peer, byte channel, INetworkMessage message)
    {
        return true;
    }
}

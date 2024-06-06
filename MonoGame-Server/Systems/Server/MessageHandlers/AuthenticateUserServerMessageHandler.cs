using LiteNetLib;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;

namespace MonoGame_Server.Systems.Server.MessageHandlers;

public class AuthenticateUserServerMessageHandler : IServerMessageHandler
{
    private readonly NetworkServer networkServer = NetworkServer.Instance;

    public bool Validate(NetPeer peer, byte channel, INetworkMessage message)
    {
        AuthenticateUserNetworkMessage authMessage = (AuthenticateUserNetworkMessage)message;
        NetPeer existingPeer = networkServer.GetPeerByUUID(authMessage.UUID);
        if (existingPeer != null)
        {
            networkServer.SendMessageToPeer(peer, new AuthenticationResultNetworkMessage(false, "Player with UUID already connected"));
            return false;
        }

        return true;
    }

    public void Execute(NetPeer peer, byte channel, INetworkMessage message)
    {
        AuthenticateUserNetworkMessage authMessage = (AuthenticateUserNetworkMessage)message;
        networkServer.Connections[peer] = authMessage.UUID;
        networkServer.SendMessageToPeer(peer, new AuthenticationResultNetworkMessage(true, "Player authenticated"));
    }
}

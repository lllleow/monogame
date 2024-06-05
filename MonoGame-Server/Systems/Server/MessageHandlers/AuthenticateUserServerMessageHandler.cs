using LiteNetLib;
using MonoGame;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;

namespace MonoGame_Server.Systems.Server.MessageHandlers;

public class AuthenticateUserServerMessageHandler : IServerMessageHandler
{
    private NetworkServer NetworkServer = NetworkServer.Instance;

    public bool Validate(NetPeer peer, byte channel, INetworkMessage message)
    {
        AuthenticateUserNetworkMessage authMessage = (AuthenticateUserNetworkMessage)message;
        NetPeer existingPeer = NetworkServer.GetPeerByUUID(authMessage.UUID);
        if (existingPeer != null)
        {
            NetworkServer.SendMessageToPeer(peer, new AuthenticationResultNetworkMessage(false, "Player with UUID already connected"));
            return false;
        }

        return true;
    }

    public void Execute(NetPeer peer, byte channel, INetworkMessage message)
    {
        AuthenticateUserNetworkMessage authMessage = (AuthenticateUserNetworkMessage)message;
        NetworkServer.Connections[peer] = authMessage.UUID;
        NetworkServer.SendMessageToPeer(peer, new AuthenticationResultNetworkMessage(true, "Player authenticated"));
    }
}

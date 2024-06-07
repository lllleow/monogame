using MonoGame;
using MonoGame_Server.Systems.Server;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.Messages.Authentication;

namespace MonoGame_Server;

public class AuthenticationNetworkServerController : IStandaloneNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<AuthenticateUserNetworkMessage>((server, peer, message) =>
        {
            server.Connections[peer] = message.UUID;
            server.SendMessageToPeer(peer, new AuthenticationResultNetworkMessage(true, "Player authenticated"));
        });
    }
}

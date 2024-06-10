using MonoGame_Common.Messages.Authentication;

namespace MonoGame_Server.Systems.Server.Controllers;

public class AuthenticationNetworkServerController : IServerNetworkController
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
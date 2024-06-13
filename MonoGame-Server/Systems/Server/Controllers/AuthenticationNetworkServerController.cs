using MonoGame_Common.Messages.Authentication;

namespace MonoGame_Server.Systems.Server.Controllers;

public class AuthenticationNetworkServerController : IServerNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<AuthenticateUserNetworkMessage>((server, peer, message) =>
        {
            if (message.UUID == null) return;
            server.Connections[peer] = message.UUID;
            NetworkServer.SendMessageToPeer(peer, new AuthenticationResultNetworkMessage()
            {
                Success = true,
                Reason = "Player authenticated"
            });
        });
    }

    public void Update()
    {
    }
}
using MonoGame;

namespace MonoGame_Server.Systems.Server.MessageHandlers;

public class AuthenticateUserServerMessageHandler : IServerMessageHandler<AuthenticateUserNetworkMessage>
{
    public void Validate(AuthenticateUserNetworkMessage message)
    {
        if (message.UUID == null)
        {
            throw new Exception("UUID is null");
        }
    }

    public void Execute(AuthenticateUserNetworkMessage message)
    {
        Console.WriteLine("Server received AuthenticateUserNetworkMessage: " + message.UUID);
    }
}

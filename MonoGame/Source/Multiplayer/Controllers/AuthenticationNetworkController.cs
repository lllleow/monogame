using System;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;
using MonoGame.Source.WorldNamespace;

namespace MonoGame;

public class AuthenticationNetworkController : IStandaloneNetworkController
{
    public void InitializeListeners()
    {
        ClientNetworkEventManager.Subscribe<AuthenticationResultNetworkMessage>(message =>
        {
            if (message.Success)
            {
                Console.WriteLine("Authentication successful!");
                NetworkClient.SendMessage(new RequestToLoadWorldNetworkMessage());
            }
            else
            {
                Console.WriteLine("Authentication failed! " + message.Reason);
            }
        });
    }
}

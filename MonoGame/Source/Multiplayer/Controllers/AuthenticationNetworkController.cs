using System;
using MonoGame_Common.Messages.Authentication;
using MonoGame_Common.Messages.Player;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame.Source.Multiplayer.Controllers;

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
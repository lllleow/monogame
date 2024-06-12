using System;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame_Common.Messages.Authentication;
using MonoGame_Common.Messages.Player;

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
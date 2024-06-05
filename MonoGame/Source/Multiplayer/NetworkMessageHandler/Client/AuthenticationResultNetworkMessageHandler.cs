using System;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;

namespace MonoGame.Source.Multiplayer.NetworkMessageHandler.Client;

public class AuthenticationResultNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        AuthenticationResultNetworkMessage authMessage = (AuthenticationResultNetworkMessage)message;

        if (authMessage.Success)
        {
            Console.WriteLine("Authentication successful!");
            NetworkClient.Instance.SendMessage(new RequestToLoadWorldNetworkMessage());
        }
        else
        {
            Console.WriteLine("Authentication failed! " + authMessage.Reason);
        }
    }
}

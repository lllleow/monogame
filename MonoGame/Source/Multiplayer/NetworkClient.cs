using System;
using LiteNetLib;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;

namespace MonoGame.Source.Multiplayer;

public class NetworkClient
{
    public static NetworkClient Instance { get; set; } = new();
    private readonly EventBasedNetListener listener;
    public NetManager Client { get; set; }

    public NetworkClient()
    {
        listener = new EventBasedNetListener();
        Client = new NetManager(listener);

        _ = Client.Start();
        _ = Client.Connect("192.168.0.123", 9050, "monogame");

        listener.PeerConnectedEvent += peer =>
        {
            AuthenticateUser();
        };

        listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
        {
            if (reader.AvailableBytes > 0)
            {
                var messageType = (NetworkMessageTypes)reader.GetByte();
                var messageObjectType = NetworkMessageTypeClientHelper.GetTypeFromMessageType(messageType);

                var message = (INetworkMessage)Activator.CreateInstance(messageObjectType);
                message.Deserialize(reader);

                var handlerType = NetworkMessageTypeClientHelper.GetHandlerForClientMessageType(messageType);
                dynamic handler = Activator.CreateInstance(handlerType);
                handler.Execute(channel, message);

                Console.WriteLine("Client Received: " + message);
            }

            reader.Recycle();
        };
    }

    public void AuthenticateUser()
    {
        var authenticateUser = new AuthenticateUserNetworkMessage(Globals.UUID);
        SendMessage(authenticateUser);
    }

    public void SendMessage(INetworkMessage message)
    {
        Console.WriteLine("Client Sent: " + message);
        Client.FirstPeer?.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public void Update()
    {
        Client.PollEvents();
    }

    public void Stop()
    {
        Client.Stop();
    }
}

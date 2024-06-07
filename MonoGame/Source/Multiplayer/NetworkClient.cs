using System;
using System.Collections.Generic;
using LiteNetLib;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.Messages.Authentication;

namespace MonoGame.Source.Multiplayer;

public static class NetworkClient
{
    private static readonly EventBasedNetListener Listener;
    public static NetManager Client { get; set; }
    public static List<INetworkController> NetworkControllers { get; set; } = new();

    static NetworkClient()
    {
        Listener = new EventBasedNetListener();
        Client = new NetManager(Listener);

        Client.Start();

        if (Globals.Args.Length > 0 && Globals.Args[0] == "localhost")
        {
            Client.Connect("localhost", 25565, "key");
        }
        else
        {
            Client.Connect("agruta.duckdns.com", 25565, "monogame");
        }

        InitializeNetworkClient();
        InitializeControllers();
        AuthenticateUser();
    }

    public static void InitializeNetworkClient()
    {
        Listener.PeerConnectedEvent += peer =>
        {
            AuthenticateUser();
        };

        Listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
        {
            if (reader.AvailableBytes > 0)
            {
                byte messageTypeId = reader.GetByte();
                Type messageType = MessageRegistry.Instance.GetTypeById((int)messageTypeId);
                INetworkMessage message = (INetworkMessage)Activator.CreateInstance(messageType);
                message.Deserialize(reader);
                ClientNetworkEventManager.RaiseEvent(messageType, message);
                Console.WriteLine("Client Received: " + message);
            }

            reader.Recycle();
        };
    }

    public static void InitializeControllers()
    {
        ClientNetworkEventManager.AddController(new AuthenticationNetworkController());
    }

    public static void AuthenticateUser()
    {
        var authenticateUser = new AuthenticateUserNetworkMessage(Globals.UUID);
        SendMessage(authenticateUser);
    }

    public static void SendMessage(INetworkMessage message)
    {
        Console.WriteLine("Client Sent: " + message);
        Client.FirstPeer?.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public static void Update()
    {
        Client.PollEvents();
    }

    public static void Stop()
    {
        Client.Stop();
    }
}

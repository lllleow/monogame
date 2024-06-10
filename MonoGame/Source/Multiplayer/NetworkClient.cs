using System;
using System.Collections.Generic;
using LiteNetLib;
using MonoGame.Source.Multiplayer.Controllers;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame_Common.Messages;
using MonoGame_Common.Messages.Authentication;
using MonoGame_Common.Util;

namespace MonoGame.Source.Multiplayer;

public static class NetworkClient
{
    private static readonly EventBasedNetListener Listener;

    static NetworkClient()
    {
        Listener = new EventBasedNetListener();
        Client = new NetManager(Listener);

        _ = Client.Start();

        _ = Globals.Args.Length > 0 && Globals.Args[0] == "localhost"
            ? Client.Connect("localhost", 25565, "key")
            : Client.Connect("189.30.162.90", 25565, "monogame");

        InitializeNetworkClient();
        InitializeControllers();
        AuthenticateUser();
    }

    public static NetManager Client { get; set; }
    public static List<INetworkController> NetworkControllers { get; set; } = [];

    public static void InitializeNetworkClient()
    {
        Listener.PeerConnectedEvent += peer => { AuthenticateUser(); };

        Listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
        {
            if (reader.AvailableBytes > 0)
            {
                var messageTypeId = reader.GetByte();
                var messageType = MessageRegistry.Instance.GetTypeById(messageTypeId);
                var message = (INetworkMessage)Activator.CreateInstance(messageType);
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
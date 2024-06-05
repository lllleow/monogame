using System;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame;

public class NetworkClient
{
    EventBasedNetListener listener;
    public NetManager client;

    public NetworkClient()
    {

        listener = new EventBasedNetListener();
        client = new NetManager(listener);

        client.Start();
        client.Connect("localhost", 9050, "monogame");

        listener.PeerConnectedEvent += peer =>
        {
            AuthenticateUser();
        };

        listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
        {
            if (reader.AvailableBytes > 0)
            {
                INetworkMessage message = (INetworkMessage)Activator.CreateInstance(NetworkMessageTypeHelper.GetTypeFromMessageType((NetworkMessageTypes)reader.GetByte()));
                message.Deserialize(reader);

                Console.Write("Client Received: " + client);
            }

            reader.Recycle();
        };
    }

    public void AuthenticateUser()
    {
        AuthenticateUserNetworkMessage authenticateUser = new AuthenticateUserNetworkMessage(Globals.UUID);
        SendMessage(authenticateUser);
    }

    public void SendMessage(INetworkMessage message)
    {
        Console.Write("Client Sent: " + message);
        client.FirstPeer.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public void Update()
    {
        client.PollEvents();
    }

    public void Stop()
    {
        client.Stop();
    }
}

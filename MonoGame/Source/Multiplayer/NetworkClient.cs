using System;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;

namespace MonoGame;

public class NetworkClient
{
    public static NetworkClient Instance = new NetworkClient();
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
                NetworkMessageTypes messageType = (NetworkMessageTypes)reader.GetByte();
                Type messageObjectType = NetworkMessageTypeClientHelper.GetTypeFromMessageType(messageType);

                INetworkMessage message = (INetworkMessage)Activator.CreateInstance(messageObjectType);
                message.Deserialize(reader);

                Type handlerType = NetworkMessageTypeClientHelper.GetHandlerForClientMessageType(messageType);
                dynamic handler = Activator.CreateInstance(handlerType);
                handler.Execute(channel, message);

                Console.WriteLine("Client Received: " + message);
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

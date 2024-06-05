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

    public void InitializeClient()
    {
        listener = new EventBasedNetListener();
        client = new NetManager(listener);

        client.Start();
        client.Connect("localhost", 9050, "monogame");

        listener.PeerConnectedEvent += peer =>
        {
            SendHandshake();
        };

        listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
        {
            if (reader.AvailableBytes > 0)
            {
                INetworkMessage message = (INetworkMessage)Activator.CreateInstance(NetworkMessageTypeHelper.GetTypeFromMessageType((NetworkMessageTypes)reader.GetByte()));
                message.Deserialize(reader);

                Console.Write("Client Received: " + client);

                if (message is IClientExecutableMessage clientMessage)
                {
                    clientMessage.ExecuteOnClient();
                }
            }

            reader.Recycle();
        };
    }

    public void SendHandshake()
    {
        HandshakeMessage handshake = new HandshakeMessage(Globals.UUID);
        Globals.networkManager.GetClient()?.SendMessage(handshake);
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

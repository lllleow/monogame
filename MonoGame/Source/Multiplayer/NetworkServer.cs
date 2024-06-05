using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame;

public class NetworkServer
{
    EventBasedNetListener listener;
    NetManager server;
    public Dictionary<NetPeer, string> Connections = new Dictionary<NetPeer, string>();

    public void InitializeServer()
    {
        listener = new EventBasedNetListener();

        server = new NetManager(listener);
        server.Start(9050);

        SetupListeners();
    }

    public void SetupListeners()
    {
        listener.ConnectionRequestEvent += request =>
        {
            if (ShouldAcceptConnection())
            {
                request.Accept();
            }
            else
                request.Reject();
        };

        listener.PeerConnectedEvent += peer =>
        {
            Console.WriteLine("New connection: {0}", peer);
        };

        listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
        {
            if (reader.AvailableBytes > 0)
            {
                INetworkMessage message = (INetworkMessage)Activator.CreateInstance(NetworkMessageTypeHelper.GetTypeFromMessageType((NetworkMessageTypes)reader.GetByte()));
                message.Deserialize(reader);

                Console.Write("Server: " + message);

                if (message is IServerExecutableMessage serverMessage)
                {
                    serverMessage.ExecuteOnServer(peer, reader, deliveryMethod, channel);
                }

                if (message is IClientExecutableMessage clientMessage)
                {
                    clientMessage.ExecuteOnClient();
                }
            }
            reader.Recycle();
        };
    }

    public NetPeer GetConnection(string UUID)
    {
        return Connections.FirstOrDefault(x => x.Value == UUID).Key;
    }

    public NetPeer RegisterConnection(string UUID, NetPeer peer)
    {
        Connections.Add(peer, UUID);
        return peer;
    }

    public void SendMessage(string uuid, INetworkMessage message)
    {
        if (message is IClientExecutableMessage clientMessage && uuid == Globals.world.GetLocalPlayer()?.UUID)
        {
            clientMessage.ExecuteOnClient();
        }

        var peer = Connections.FirstOrDefault(x => x.Value == uuid).Key;
        if (peer != null)
        {
            peer.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
        }
    }

    public void BroadcastMessage(INetworkMessage message)
    {
        if (message is IClientExecutableMessage clientMessage)
        {
            clientMessage.ExecuteOnClient();
        }

        foreach (var peer in Connections.Keys)
        {
            peer.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
        }
    }

    public bool ShouldAcceptConnection()
    {
        return server.ConnectedPeersCount < 10;
    }

    public void Update()
    {
        server.PollEvents();
    }

    public void Stop()
    {
        server.Stop();
    }
}

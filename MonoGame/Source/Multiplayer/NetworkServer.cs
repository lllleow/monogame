using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

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

        listener.ConnectionRequestEvent += request =>
        {
            if (server.ConnectedPeersCount < 10)
            {
                request.Accept();
            }
            else
                request.Reject();
        };

        listener.PeerConnectedEvent += peer =>
        {
            Console.WriteLine("We got connection: {0}", peer);
            NetDataWriter writer = new NetDataWriter();
            writer.Put("Hello client!");
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        };

        listener.NetworkReceiveEvent += (fromPeer, reader, deliveryMethod, channel) =>
        {
            if (reader.AvailableBytes > 0)
            {
                byte messageType = reader.GetByte();
                if (messageType == 1)
                {
                    string uuid = reader.GetString();
                    Player player = new Player(new Vector2(500, 500))
                    {
                        UUID = uuid
                    };

                    Globals.world.Players.Add(player);
                    Connections.Add(fromPeer, uuid);
                }

                if (messageType == 2)
                {
                    float posX = reader.GetFloat();
                    float posY = reader.GetFloat();
                    string uuid = Connections[fromPeer];

                    Player player = Globals.world.Players.Where(p => p.UUID == uuid).First();
                    player.Position = new Vector2(posX, posY);

                    foreach (var p in Connections)
                    {
                        NetDataWriter writer = new NetDataWriter();
                        writer.Put(2);
                        writer.Put(player.UUID);
                        writer.Put(player.Position.X);
                        writer.Put(player.Position.Y);
                        p.Key.Send(writer, DeliveryMethod.ReliableOrdered);
                    }
                }
            }
            reader.Recycle();
        };
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

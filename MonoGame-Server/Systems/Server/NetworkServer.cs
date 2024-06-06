using LiteNetLib;
using MonoGame_Server.Systems.World;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.WorldNamespace.WorldStates;
namespace MonoGame_Server.Systems.Server;

public class NetworkServer
{
    public static NetworkServer Instance = new();
    private readonly EventBasedNetListener listener;
    private readonly NetManager server;
    public Dictionary<NetPeer, string> Connections = [];
    public MessageHandler MessageHandler = new();
    public ServerWorld ServerWorld;

    public NetworkServer()
    {
        listener = new EventBasedNetListener();
        server = new NetManager(listener);
        ServerWorld = new ServerWorld();
        ServerWorld.Initialize();
    }

    public void InitializeServer()
    {
        Console.WriteLine("Initializing server");
        int port = 9050;
        _ = server.Start(port);

        Console.WriteLine("Server started at port " + port);
        SetupListeners();
    }

    public NetPeer GetPeerByUUID(string UUID)
    {
        return Connections.FirstOrDefault(x => x.Value == UUID).Key;
    }

    public string GetUUIDByPeer(NetPeer peer)
    {
        return Connections.FirstOrDefault(x => x.Key == peer).Value;
    }

    public PlayerState? GetPlayerFromPeer(NetPeer peer)
    {
        return ServerWorld.GetPlayerByUUID(GetUUIDByPeer(peer));
    }

    public void SetupListeners()
    {
        Console.WriteLine("Server is listening for connections");
        listener.ConnectionRequestEvent += request =>
        {
            if (ShouldAcceptConnection())
            {
                _ = request.Accept();
            }
            else
            {
                request.Reject();
            }
        };

        listener.PeerConnectedEvent += peer =>
        {
            Console.WriteLine("New connection: {0}", peer);
        };

        listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
        {
            Console.WriteLine("Network message received from {0}", peer.Address);
            if (reader.AvailableBytes > 0)
            {
                MessageHandler.HandleMessage(peer, reader, deliveryMethod, channel);
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
        var peer = Connections.FirstOrDefault(x => x.Value == uuid).Key;
        peer?.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public void SendMessageToPeer(NetPeer peer, INetworkMessage message)
    {
        peer.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public void BroadcastMessage(INetworkMessage message)
    {
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
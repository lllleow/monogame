using LiteNetLib;
using MonoGame;
using MonoGame_Server.Systems.World;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.WorldNamespace.WorldStates;
namespace MonoGame_Server.Systems.Server;

public class NetworkServer
{
    public static NetworkServer Instance { get; set; } = new();
    private readonly EventBasedNetListener listener;
    private readonly NetManager server;
    public Dictionary<NetPeer, string> Connections { get; set; } = [];
    public ServerWorld ServerWorld { get; set; }
    public List<INetworkController> NetworkControllers { get; set; } = new();

    public NetworkServer()
    {
        listener = new EventBasedNetListener();
        server = new NetManager(listener);
        InitializeControllers();
        ServerWorld = new ServerWorld();
        ServerWorld.Initialize();
    }

    public void InitializeServer()
    {
        Console.WriteLine("Initializing server");
        int port = 25565;
        _ = server.Start(port);

        Console.WriteLine("Server started at port " + port);

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
                byte messageTypeId = reader.GetByte();
                Type messageType = MessageRegistry.Instance.GetTypeById((int)messageTypeId);
                INetworkMessage? message = (INetworkMessage?)Activator.CreateInstance(messageType);
                message?.Deserialize(reader);
                if (message != null)
                {
                    ServerNetworkEventManager.RaiseEvent(this, peer, messageType, message);
                }

                Console.WriteLine("Server received: " + message);
            }

            reader.Recycle();
        };
    }

    public void InitializeControllers()
    {
        ServerNetworkEventManager.AddController(new AuthenticationNetworkServerController());
        ServerNetworkEventManager.AddController(new PlayerNetworkServerController());
        ServerNetworkEventManager.AddController(new WorldNetworkServerController());
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
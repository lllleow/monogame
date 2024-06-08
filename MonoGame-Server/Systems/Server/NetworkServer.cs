using LiteNetLib;
using MonoGame;
using MonoGame_Server.Systems.World;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.States;

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
        this.listener = new EventBasedNetListener();
        this.server = new NetManager(this.listener);
        this.InitializeControllers();
        this.ServerWorld = new ServerWorld();
        this.ServerWorld.Initialize();
    }

    public void InitializeServer()
    {
        Console.WriteLine("Initializing server");
        int port = 25565;
        _ = this.server.Start(port);

        Console.WriteLine("Server started at port " + port);

        Console.WriteLine("Server is listening for connections");
        this.listener.ConnectionRequestEvent += request =>
        {
            if (this.ShouldAcceptConnection())
            {
                _ = request.Accept();
            }
            else
            {
                request.Reject();
            }
        };

        this.listener.PeerConnectedEvent += peer =>
        {
            Console.WriteLine("New connection: {0}", peer);
        };

        this.listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
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
        ServerNetworkEventManager.AddController(new AnimatorComponentNetworkServerController());
        ServerNetworkEventManager.AddController(new ComponentNetworkServerController());
    }

    public NetPeer GetPeerByUUID(string UUID)
    {
        return this.Connections.FirstOrDefault(x => x.Value == UUID).Key;
    }

    public string GetUUIDByPeer(NetPeer peer)
    {
        return this.Connections.FirstOrDefault(x => x.Key == peer).Value;
    }

    public PlayerState? GetPlayerFromPeer(NetPeer peer)
    {
        return this.ServerWorld.GetPlayerByUUID(this.GetUUIDByPeer(peer));
    }

    public NetPeer GetConnection(string UUID)
    {
        return this.Connections.FirstOrDefault(x => x.Value == UUID).Key;
    }

    public NetPeer RegisterConnection(string UUID, NetPeer peer)
    {
        this.Connections.Add(peer, UUID);
        return peer;
    }

    public void SendMessage(string uuid, INetworkMessage message)
    {
        var peer = this.Connections.FirstOrDefault(x => x.Value == uuid).Key;
        peer?.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public void SendMessageToPeer(NetPeer peer, INetworkMessage message)
    {
        peer.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public void BroadcastMessage(INetworkMessage message, List<NetPeer>? blacklist = null)
    {
        blacklist ??= new List<NetPeer>();
        List<NetPeer> whitelistedPeers = this.Connections.Keys.Except(blacklist).ToList();

        foreach (var peer in whitelistedPeers)
        {
            peer.Send(message.Serialize(), DeliveryMethod.Unreliable);
        }
    }

    public bool ShouldAcceptConnection()
    {
        return this.server.ConnectedPeersCount < 10;
    }

    public void Update()
    {
        this.server.PollEvents();
    }

    public void Stop()
    {
        this.server.Stop();
    }
}
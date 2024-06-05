using LiteNetLib;
using MonoGame.Source.Multiplayer.Interfaces;
namespace MonoGame_Server.Systems.Server;

public class NetworkServer
{
    public static NetworkServer Instance = new NetworkServer();

    EventBasedNetListener listener;
    NetManager server;
    public Dictionary<NetPeer, string> Connections = new Dictionary<NetPeer, string>();
    public MessageHandler MessageHandler = new MessageHandler();

    public NetworkServer()
    {
        listener = new EventBasedNetListener();
        server = new NetManager(listener);
    }

    public void InitializeServer()
    {
        Console.WriteLine("Initializing server");
        int port = 9050;
        server.Start(port);

        Console.WriteLine("Server started at port " + port);
        SetupListeners();
    }

    public void SetupListeners()
    {
        Console.WriteLine("Server is listening for connections");
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
        if (peer != null)
        {
            peer.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
        }
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
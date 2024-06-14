using System.Collections.Concurrent;
using LiteNetLib;
using MonoGame_Common.Messages;
using MonoGame_Common.States;
using MonoGame_Common.Systems.Scripts;
using MonoGame_Common.Util;
using MonoGame_Server.Systems.Saving;
using MonoGame_Server.Systems.Server.Controllers;
using MonoGame_Server.Systems.Server.Controllers.Components;
using MonoGame_Server.Systems.World;

namespace MonoGame_Server.Systems.Server;

public class NetworkServer
{
    private readonly EventBasedNetListener listener;
    private readonly NetManager server;
    private int autoSaveCounter;

    public NetworkServer()
    {
        listener = new EventBasedNetListener();
        server = new NetManager(listener);
    }

    public static NetworkServer Instance { get; set; } = new()
    {
        ServerWorld = new ServerWorld()
    };

    public ConcurrentDictionary<NetPeer, string> Connections { get; set; } = [];
    required public ServerWorld ServerWorld { get; set; }

    public void InitializeServer()
    {
        Console.WriteLine("Initializing server");
        var port = 25565;
        _ = server.Start(port);

        Console.WriteLine("Server started at port " + port);

        Console.WriteLine("Loading scripts");

        TileRegistry.LoadTileScripts();
        AnimationBundleRegistry.LoadAnimationBundleScripts();

        Console.WriteLine("Finished loading scripts");

        InitializeControllers();
        ServerWorld.Initialize();

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

        listener.PeerConnectedEvent += peer => { Console.WriteLine("New connection: {0}", peer); };

        listener.NetworkReceiveEvent += (peer, reader, channel, deliveryMethod) =>
        {
            Console.WriteLine("Network message received from {0}", peer.Address);
            if (reader.AvailableBytes > 0)
            {
                var messageTypeId = reader.GetByte();
                var messageType = MessageRegistry.Instance.GetTypeById(messageTypeId);
                var message = (INetworkMessage?)Activator.CreateInstance(messageType);
                message?.Deserialize(reader);
                if (message != null)
                {
                    ServerNetworkEventManager.RaiseEvent(this, peer, messageType, message);
                }

                Console.WriteLine("Server received: " + message);
            }

            reader.Recycle();
        };

        ServerWorld.UpdateTextureCoordinates();
    }

    public void SetEntity(EntityState entity)
    {
        ServerWorld?.Entities?.Remove(entity);
        ServerWorld?.Entities?.Add(entity);
    }

    public static void InitializeControllers()
    {
        ServerNetworkEventManager.AddController(new AuthenticationNetworkServerController());
        ServerNetworkEventManager.AddController(new PlayerNetworkServerController());
        ServerNetworkEventManager.AddController(new WorldNetworkServerController());
        ServerNetworkEventManager.AddController(new AnimatorComponentNetworkServerController());
        ServerNetworkEventManager.AddController(new ComponentNetworkServerController());
        ServerNetworkEventManager.AddController(new CollisionComponentNetworkServerController());
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
        Connections.TryAdd(peer, UUID);
        return peer;
    }

    public void SendMessage(string uuid, INetworkMessage message)
    {
        var peer = Connections.FirstOrDefault(x => x.Value == uuid).Key;
        peer?.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public static void SendMessageToPeer(NetPeer peer, INetworkMessage message)
    {
        peer.Send(message.Serialize(), DeliveryMethod.ReliableOrdered);
    }

    public void BroadcastMessage(INetworkMessage message, List<NetPeer>? blacklist = null)
    {
        blacklist ??= [];
        var whitelistedPeers = Connections.Keys.Except(blacklist).ToList();

        foreach (var peer in whitelistedPeers)
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

        if (autoSaveCounter >= 1000)
        {
            autoSaveCounter = 0;
            SaveManager.SaveGame();
        }
        else
        {
            autoSaveCounter++;
        }

        foreach (var controller in ServerNetworkEventManager.NetworkControllers ?? [])
        {
            controller.Update();
        }
    }

    public void Stop()
    {
        server.Stop();
    }
}
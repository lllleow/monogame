using LiteNetLib;
using MonoGame_Common.Messages;

namespace MonoGame_Server.Systems.Server;

public static class ServerNetworkEventManager
{
    private static readonly Dictionary<Type, Action<NetworkServer, NetPeer, INetworkMessage>> _subscriptions;
    private static readonly List<IServerNetworkController> _controllers = [];

    static ServerNetworkEventManager()
    {
        _subscriptions = [];
    }

    public static void AddController(IServerNetworkController controller)
    {
        _controllers.Add(controller);
        controller.InitializeListeners();
    }

    public static void Subscribe<T>(Action<NetworkServer, NetPeer, T> handler)
        where T : INetworkMessage
    {
        var messageType = typeof(T);
        if (!_subscriptions.ContainsKey(messageType))
            _subscriptions[messageType] = (server, peer, msg) => handler(server, peer, (T)msg);
        else
            _subscriptions[messageType] += (server, peer, msg) => handler(server, peer, (T)msg);
    }

    public static void RaiseEvent<T>(NetworkServer server, NetPeer peer, Type messageType, T message)
        where T : INetworkMessage
    {
        if (_subscriptions.TryGetValue(messageType, out var handlers)) handlers(server, peer, message);
    }

    public static void Unsubscribe<T>(Action<NetworkServer, NetPeer, T> handler)
        where T : INetworkMessage
    {
        var messageType = typeof(T);
        if (_subscriptions.TryGetValue(messageType, out var currentHandlers))
        {
            currentHandlers -= (server, peer, msg) => handler(server, peer, (T)msg);
            if (currentHandlers == null)
                _ = _subscriptions.Remove(messageType);
            else
                _subscriptions[messageType] = currentHandlers;
        }
    }
}
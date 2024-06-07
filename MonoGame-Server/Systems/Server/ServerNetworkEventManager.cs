using System;

namespace MonoGame;

using System;
using System.Collections.Generic;
using LiteNetLib;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame_Server;
using MonoGame_Server.Systems.Server;

public static class ServerNetworkEventManager
{
    private static Dictionary<Type, Action<NetworkServer, NetPeer, INetworkMessage>> _subscriptions;
    private static List<INetworkController> _controllers = new();

    static ServerNetworkEventManager()
    {
        _subscriptions = new Dictionary<Type, Action<NetworkServer, NetPeer, INetworkMessage>>();
    }

    public static void AddController(INetworkController controller)
    {
        if (controller is IStandaloneNetworkController)
        {
            _controllers.Add(controller);
            (controller as IStandaloneNetworkController)?.InitializeListeners();
        }
    }

    public static void Subscribe<T>(Action<NetworkServer, NetPeer, T> handler)
        where T : INetworkMessage
    {
        var messageType = typeof(T);
        if (!_subscriptions.ContainsKey(messageType))
        {
            _subscriptions[messageType] = (server, peer, msg) => handler(server, peer, (T)msg);
        }
        else
        {
            _subscriptions[messageType] += (server, peer, msg) => handler(server, peer, (T)msg);
        }
    }

    public static void RaiseEvent<T>(NetworkServer server, NetPeer peer, Type messageType, T message)
        where T : INetworkMessage
    {
        if (_subscriptions.TryGetValue(messageType, out var handlers))
        {
            handlers(server, peer, message);
        }
    }

    public static void Unsubscribe<T>(Action<NetworkServer, NetPeer, T> handler)
        where T : INetworkMessage
    {
        var messageType = typeof(T);
        if (_subscriptions.TryGetValue(messageType, out var currentHandlers))
        {
            currentHandlers -= (server, peer, msg) => handler(server, peer, (T)msg);
            if (currentHandlers == null)
            {
                _subscriptions.Remove(messageType);
            }
            else
            {
                _subscriptions[messageType] = currentHandlers;
            }
        }
    }
}
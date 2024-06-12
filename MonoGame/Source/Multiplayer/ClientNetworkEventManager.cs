using System;
using System.Collections.Generic;
using MonoGame_Common.Messages;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame.Source.Multiplayer;

public static class ClientNetworkEventManager
{
    private static readonly Dictionary<Type, Action<INetworkMessage>> _subscriptions;
    private static readonly List<INetworkController> _controllers = [];

    static ClientNetworkEventManager()
    {
        _subscriptions = [];
    }

    public static void AddController(INetworkController controller)
    {
        if (controller is IStandaloneNetworkController)
        {
            _controllers.Add(controller);
            (controller as IStandaloneNetworkController)?.InitializeListeners();
        }
    }

    public static void Subscribe<T>(Action<T> handler)
        where T : INetworkMessage
    {
        var messageType = typeof(T);
        if (!_subscriptions.ContainsKey(messageType))
        {
            _subscriptions[messageType] = msg => handler((T)msg);
        }
        else
        {
            _subscriptions[messageType] += msg => handler((T)msg);
        }
    }

    public static void RaiseEvent<T>(Type messageType, T message)
        where T : INetworkMessage
    {
        if (_subscriptions.TryGetValue(messageType, out var handlers))
        {
            handlers(message);
        }
    }

    public static void Unsubscribe<T>(Action<T> handler)
        where T : INetworkMessage
    {
        var messageType = typeof(T);
        if (_subscriptions.TryGetValue(messageType, out var currentHandlers))
        {
            currentHandlers -= msg => handler((T)msg);
            if (currentHandlers == null)
            {
                _ = _subscriptions.Remove(messageType);
            }
            else
            {
                _subscriptions[messageType] = currentHandlers;
            }
        }
    }
}
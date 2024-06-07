using System;

namespace MonoGame;

using System;
using System.Collections.Generic;
using MonoGame.Source.Multiplayer.Interfaces;

public static class ClientNetworkEventManager
{
    private static Dictionary<Type, Action<INetworkMessage>> _subscriptions;
    private static List<INetworkController> _controllers = new();

    static ClientNetworkEventManager()
    {
        _subscriptions = new Dictionary<Type, Action<INetworkMessage>>();
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
        Type messageType = typeof(T);
        if (!_subscriptions.ContainsKey(messageType))
        {
            _subscriptions[messageType] = (msg) => handler((T)msg);
        }
        else
        {
            _subscriptions[messageType] += (msg) => handler((T)msg);
        }
    }

    public static void RaiseEvent<T>(Type messageType, T message)
        where T : INetworkMessage
    {
        if (_subscriptions.TryGetValue(messageType, out Action<INetworkMessage> handlers))
        {
            handlers(message);
        }
    }

    public static void Unsubscribe<T>(Action<T> handler)
        where T : INetworkMessage
    {
        Type messageType = typeof(T);
        if (_subscriptions.TryGetValue(messageType, out Action<INetworkMessage> currentHandlers))
        {
            currentHandlers -= (msg) => handler((T)msg);
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
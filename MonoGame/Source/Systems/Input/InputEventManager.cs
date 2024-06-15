using System;
using System.Collections.Generic;
using MonoGame_Common.Messages;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame;

public class InputEventManager
{
    private static Dictionary<InputEventChannel, List<Action<InputEvent>>> subscriptions = new();

    public static void Subscribe(InputEventChannel channel, Action<InputEvent> handler)
    {
        if (!subscriptions.ContainsKey(channel))
        {
            subscriptions[channel] = new List<Action<InputEvent>>();
        }

        subscriptions[channel].Add(handler);
    }

    public static void RaiseEvent(InputEvent message)
    {
        foreach (var channel in subscriptions.Keys)
        {
            RaiseEvent(channel, message);
        }
    }

    public static void RaiseEvent(InputEventChannel channel, InputEvent message)
    {
        foreach (var subscription in subscriptions[channel])
        {
            subscription(message);
        }
    }

    public static void Unsubscribe(InputEventChannel channel, Action<InputEvent> handler)
    {
        subscriptions[channel].Remove(handler);
    }
}

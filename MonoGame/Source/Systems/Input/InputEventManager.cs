using System;
using System.Collections.Generic;
using MonoGame_Common.Messages;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame;

public class InputEventManager
{
    private static List<Action<InputEvent>> subscriptions = new();

    public static void Subscribe(Action<InputEvent> handler)
    {
        subscriptions.Add(handler);
    }

    public static void RaiseEvent(InputEvent message)
    {
        foreach (var subscription in subscriptions)
        {
            subscription(message);
        }
    }

    public static void Unsubscribe(Action<InputEvent> handler)
    {
        subscriptions.Remove(handler);
    }
}

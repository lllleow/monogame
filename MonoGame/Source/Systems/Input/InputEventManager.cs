using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame;

public class InputEventManager
{
    private static Dictionary<InputEventChannel, List<(int Priority, Action<InputEvent> Handler)>> subscriptions = new();

    public static void Subscribe(InputEventChannel channel, Action<InputEvent> handler, int priority = 0)
    {
        if (!subscriptions.ContainsKey(channel))
        {
            subscriptions[channel] = new List<(int, Action<InputEvent>)>();
        }

        subscriptions[channel].Add((priority, handler));
    }

    public static void RaiseEvent(InputEvent message)
    {
        foreach (var channel in subscriptions.Keys)
        {
            var orderedSubscriptions = subscriptions[channel].OrderBy(subscription => subscription.Priority);
            foreach (var subscription in orderedSubscriptions)
            {
                if (message.Handled) return;
                subscription.Handler(message);
            }
        }
    }

    public static void RaiseEvent(InputEventChannel channel, InputEvent message)
    {
        foreach (var subscription in subscriptions[channel])
        {
            subscription.Handler(message);
        }
    }

    public static void Unsubscribe(InputEventChannel channel, Action<InputEvent> handler)
    {
        subscriptions[channel].RemoveAll(subscription => subscription.Handler == handler);
    }
}

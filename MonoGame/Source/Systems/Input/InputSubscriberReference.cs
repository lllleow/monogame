using System;

namespace MonoGame;

public class InputSubscriberReference
{
    public InputEventChannel Channel { get; set; }
    public int Priority { get; set; }
    public Action<InputEvent> Handler { get; set; }

    public InputSubscriberReference(InputEventChannel channel, int priority, Action<InputEvent> handler)
    {
        Channel = channel;
        Priority = priority;
        Handler = handler;
    }
}

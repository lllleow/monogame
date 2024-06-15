using System;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;

namespace MonoGame;

public class InputEvent
{
    public InputEventType EventType { get; set; }
    public Keys? Key { get; set; }
    public MouseButton? Button { get; set; }
    public bool Handled { get; set; } = false;
}
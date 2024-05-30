using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame;

public interface IMultipleChildUserInterfaceComponent
{
    public List<IUserInterfaceComponent> Children { get; set; }
    public void AddChild(IUserInterfaceComponent child);
    public void RemoveChild(IUserInterfaceComponent child);
    public Vector2 GetOffsetForChild(IUserInterfaceComponent child);
    public Rectangle GetBoundsOfChildren(List<IUserInterfaceComponent> excluding = null);
}

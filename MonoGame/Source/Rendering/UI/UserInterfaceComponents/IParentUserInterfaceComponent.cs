using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

public interface IParentUserInterfaceComponent
{
    public UserInterfaceAlignment ChildAlignment { get; set; }
    public abstract Vector2 GetOriginForAlignment();
}

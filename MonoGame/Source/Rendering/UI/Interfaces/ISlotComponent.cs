using System;
using MonoGame.Source.Rendering.Utils;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface ISlotComponent : IUserInterfaceComponent
{
    public abstract TextureLocation GetDrawable();
}

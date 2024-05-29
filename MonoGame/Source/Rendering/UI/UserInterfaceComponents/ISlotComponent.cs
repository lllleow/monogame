using System;

namespace MonoGame;

public interface ISlotComponent : IUserInterfaceComponent
{
    public abstract TextureLocation GetDrawable();
}

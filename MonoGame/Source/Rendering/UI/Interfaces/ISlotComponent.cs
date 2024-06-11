using MonoGame_Common.Util.Helpers;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface ISlotComponent : IUserInterfaceComponent
{
    public TextureLocation GetDrawable();
}
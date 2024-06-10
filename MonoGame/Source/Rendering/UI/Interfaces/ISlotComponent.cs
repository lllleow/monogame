using MonoGame.Source.Rendering.Utils;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface ISlotComponent : IUserInterfaceComponent
{
    public TextureLocation GetDrawable();
}
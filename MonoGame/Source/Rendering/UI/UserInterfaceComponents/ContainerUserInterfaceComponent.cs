using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class ContainerUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public ContainerUserInterfaceComponent(Vector2 localPosition, IUserInterfaceComponent child) : base("container",
        localPosition, child)
    {
    }
}
using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class ScrollViewIndicatorUserIntefaceComponent : ContainerUserInterfaceComponent
{
    public float Height { get; set; } = 0;
    public ScrollViewIndicatorUserIntefaceComponent(float height) : base(new Vector2(0, 0), null)
    {
        Height = height;
        BackgroundImage = "textures/ui_background";
        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile;
    }

    public override Microsoft.Xna.Framework.Vector2 GetPreferredSize()
    {
        Vector2 size = new Vector2(6, Height);
        CalculatedSize = size;
        return size;
    }
}

using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class PaddingUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    private int PaddingLeft { get; set; }
    private int PaddingRight { get; set; }
    private int PaddingUp { get; set; }
    private int PaddingDown { get; set; }

    public PaddingUserInterfaceComponent(int paddingLeft, int paddingRight, int paddingTop, int paddingBottom, IUserInterfaceComponent child) : base("padding", new Vector2(0, 0), child)
    {
        PaddingLeft = paddingLeft;
        PaddingRight = paddingRight;
        PaddingUp = paddingTop;
        PaddingDown = paddingBottom;
    }

    public override Vector2 GetPositionRelativeToParent()
    {
        return base.GetPositionRelativeToParent() + new Vector2(PaddingLeft, PaddingUp);
    }

    public override Vector2 GetPreferredSize()
    {
        return base.GetPreferredSize() + new Vector2(PaddingLeft + PaddingRight, PaddingUp + PaddingDown);
    }
}

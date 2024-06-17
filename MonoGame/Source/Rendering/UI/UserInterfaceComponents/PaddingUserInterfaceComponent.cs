using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class PaddingUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public PaddingUserInterfaceComponent(int paddingLeft, int paddingRight, int paddingTop, int paddingBottom, IUserInterfaceComponent child) : base("padding", new Vector2(0, 0), child)
    {
        PaddingLeft = paddingLeft;
        PaddingRight = paddingRight;
        PaddingUp = paddingTop;
        PaddingDown = paddingBottom;
    }

    private int PaddingLeft { get; }
    private int PaddingRight { get; }
    private int PaddingUp { get; }
    private int PaddingDown { get; }

    public override Vector2 GetPositionRelativeToParent()
    {
        return base.GetPositionRelativeToParent() + new Vector2(PaddingLeft, PaddingUp);
    }

    public override Vector2 GetPreferredSize()
    {
        Vector2 size = base.GetPreferredSize() + new Vector2(PaddingLeft + PaddingRight, PaddingUp + PaddingDown);
        CalculatedSize = size;
        return size;
    }
}
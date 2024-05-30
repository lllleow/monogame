using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class PaddingUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    int PaddingLeft { get; set; }
    int PaddingRight { get; set; }
    int PaddingUp { get; set; }
    int PaddingDown { get; set; }

    public PaddingUserInterfaceComponent(Vector2 localPosition, int paddingLeft, int paddingRight, int paddingTop, int paddingBottom, IUserInterfaceComponent child) : base("padding", localPosition, child)
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

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class AlignmentUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public AlignmentUserInterfaceComponent(Vector2 localPosition, IUserInterfaceComponent child) : base("alignment", localPosition, child)
    {

    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 origin)
    {
        if (Parent != null)
        {
            origin = origin + (Parent.GetPreferredSize() / 2) - (Child.GetPreferredSize() / 2) + GetPositionRelativeToParent();
        }

        base.Draw(spriteBatch, origin);
    }
}

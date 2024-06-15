using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using MonoGame_Common;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using System.Linq;

namespace MonoGame;

public class ScrollViewUserInterfaceComponent : DirectionalListUserInterfaceComponent
{
    public Vector2 ContentSize { get; set; } = Vector2.Zero;
    public Vector2 ContentOffset { get; set; } = Vector2.Zero;
    private ScrollViewIndicatorUserIntefaceComponent scrollViewIndicator;

    public ScrollViewUserInterfaceComponent(Vector2 size, IUserInterfaceComponent child) : base("scroll_view", ListDirection.Horizontal, Vector2.Zero, 2, [])
    {
        SizeOverride = size;
        scrollViewIndicator = new ScrollViewIndicatorUserIntefaceComponent(size.Y);

        ContentSize = child.GetPreferredSize();

        AddChild(child);
        AddChild(scrollViewIndicator);
    }

    public override void Initialize(IUserInterfaceComponent parent)
    {
        base.Initialize(parent);
        InputEventManager.Subscribe(InputEventChannel.UI, inputEvent =>
        {
            if (inputEvent.EventType == InputEventType.MouseScrolled)
            {
                inputEvent.Handled = true;
                float yOffset = inputEvent.ScrollDelta / 10;

                Vector2 newContentOffset = ContentOffset + new Vector2(0, yOffset);
                if (newContentOffset.Y < -ContentSize.Y)
                {
                    newContentOffset = new Vector2(newContentOffset.X, -ContentSize.Y);
                }

                if (newContentOffset.Y > 0)
                {
                    newContentOffset = new Vector2(newContentOffset.X, 0);
                }

                ContentOffset = newContentOffset;
            }
        });
    }

    public override Vector2 GetPreferredSize()
    {
        Vector2 childSize = new Vector2(0, 0);
        foreach (IUserInterfaceComponent child in Children)
        {
            childSize += child.GetPreferredSize();
        }

        if (SizeOverride != Vector2.Zero)
        {
            if (SizeOverride.X < 0 && SizeOverride.Y > 0)
            {
                return new Vector2(childSize.X, SizeOverride.Y);
            }
            else if (SizeOverride.X > 0 && SizeOverride.Y < 0)
            {
                return new Vector2(SizeOverride.X, childSize.Y);
            }

            return SizeOverride;
        }
        else
        {
            return childSize;
        }
    }

    public override Vector2 GetChildOffset(IUserInterfaceComponent child)
    {
        if (child == scrollViewIndicator)
        {
            return base.GetChildOffset(child);
        }
        else
        {
            return base.GetChildOffset(child) + ContentOffset;
        }
    }
}

using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using MonoGame.Source;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class ButtonUserInterfaceComponent : ContainerUserInterfaceComponent
{
    public string Text { get; set; } = string.Empty;
    public bool IsClicked { get; set; } = false;

    public ButtonUserInterfaceComponent(string text, Action<IUserInterfaceComponent> onClick) : base(new Vector2(0, 0), null)
    {
        Text = text;
        BackgroundImage = "textures/ui_background";
        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile;

        OnClick = onClick;

        SetChild(new AlignmentUserInterfaceComponent(
            alignment: UserInterfaceAlignment.Center,
            child: new PaddingUserInterfaceComponent(
                4,
                4,
                4,
                4,
                child: new LabelUserInterfaceComponent(Text, new Vector2(0, 0))
            )
        ));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsClicked)
        {
            BackgroundImage = "textures/ui_background_selected";
        }
        else
        {
            BackgroundImage = "textures/ui_background";
        }
    }
}

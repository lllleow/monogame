using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using MonoGame.Source;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class LevelEditorZoomSelectorUserInterfaceComponent : ContainerUserInterfaceComponent
{
    public LevelEditorZoomSelectorUserInterfaceComponent() : base(new Vector2(0, 0), null)
    {
        SetChild(new PaddingUserInterfaceComponent(
            8,
            8,
            8,
            8,
            child: new ContainerUserInterfaceComponent(
                new Vector2(0, 0),
                new PaddingUserInterfaceComponent(
                    4,
                    4,
                    4,
                    4,
                    child: new DirectionalListUserInterfaceComponent(
                        "list",
                        spacing: 2,
                        localPosition: new Vector2(0, 0),
                        direction: ListDirection.Horizontal,
                        children: new List<IUserInterfaceComponent>()
                        {
                            new ButtonUserInterfaceComponent("+", component =>
                            {
                                Globals.Camera.ScaleFactor += 0.25f;
                            })
                            {
                                IsClicked = true,
                                SizeOverride = new Vector2(15, 15)
                            },
                            new ButtonUserInterfaceComponent("-", component =>
                            {
                                Globals.Camera.ScaleFactor -= 0.25f;
                            })
                            {
                                IsClicked = true,
                                SizeOverride = new Vector2(15, 15)
                            },
                        }
                    )
                )
            )
            {
                BackgroundImage = "textures/ui_background",
                BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile
            })
        );
    }
}

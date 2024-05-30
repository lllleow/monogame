﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class LevelEditorUserInterface : UserInterface
{
    public LevelEditorUserInterface()
    {
        Name = "level_editor_user_interface";

        AddComponent(
            new ContainerUserInterfaceComponent(
                "container",
                position: new Vector2(0, 0),
                size: new Vector2(100, 100),
                contentPadding: new Vector2(0, 0),
                childAlignment: UserInterfaceAlignment.RightCenter,
                new DirectionalListUserInterfaceComponent(
                    "row",
                    ListDirection.Horizontal,
                    position: new Vector2(0, 0),
                    size: new Vector2(16, 16),
                    childAlignment: UserInterfaceAlignment.LeftUp,
                    children: new List<IUserInterfaceComponent>
                    {
                        new TileSlotComponent("tile_slot", position: new Vector2(0, 0), size: new Vector2(16, 16))
                    }
                )
            )
        );
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}

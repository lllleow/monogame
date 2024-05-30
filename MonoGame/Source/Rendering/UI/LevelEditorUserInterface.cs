using System;
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
                position: new Vector2(10, 10),
                size: new Vector2(100, 100),
                new GridUserInterfaceComponent(
                            name: "column",
                            spacing: new Vector2(8, 8),
                            columns: 3,
                            rows: 2,
                            position: new Vector2(0, 0),
                            size: new Vector2(64, 64),
                            children: new List<IUserInterfaceComponent>
                            {
                                new TileSlotComponent("tile_slot", position: new Vector2(0, 0), size: new Vector2(16, 16)),
                                new TileSlotComponent("tile_slot", position: new Vector2(0, 0), size: new Vector2(16, 16)),
                                new TileSlotComponent("tile_slot", position: new Vector2(0, 0), size: new Vector2(16, 16)),
                                new TileSlotComponent("tile_slot", position: new Vector2(0, 0), size: new Vector2(16, 16)),
                            }
                        )
            )
        );
    }
}

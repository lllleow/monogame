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

        // AddComponent(
        //     new ContainerUserInterfaceComponent(
        //         "container",
        //         position: new Vector2(0, 0),
        //         size: new Vector2(100, 100),
        //         contentPadding: new Vector2(0, 0),
        //         childAlignment: UserInterfaceAlignment.RightDown,
        //         new DirectionalListUserInterfaceComponent(
        //             "row",
        //             ListDirection.Vertical,
        //             spacing: 0,
        //             position: new Vector2(0, 0),
        //             size: new Vector2(80, 80),
        //             childAlignment: UserInterfaceAlignment.LeftUp,
        //             children: new List<IUserInterfaceComponent>
        //             {
        //                 new TileSlotComponent("tile_slot", position: new Vector2(0, 0), size: new Vector2(16, 16)),
        //                 new TileSlotComponent("tile_slot", position: new Vector2(0, 0), size: new Vector2(16, 16)),
        //                 new GridUserInterfaceComponent(
        //                     name: "column",
        //                     columns: 2,
        //                     rows: 2,
        //                     position: new Vector2(0, 0),
        //                     size: new Vector2(32, 32),
        //                     childAlignment: UserInterfaceAlignment.LeftUp,
        //                     children: new List<IUserInterfaceComponent>
        //                     {
        //                         new TileSlotComponent("tile_slot", position: new Vector2(0, 0), size: new Vector2(16, 16))
        //                     }
        //                 )
        //             }
        //         )
        //     )
        // );

        AddComponent(
            new ContainerUserInterfaceComponent(
                "container",
                position: new Vector2(0, 0),
                size: new Vector2(100, 100),
                contentPadding: new Vector2(0, 0),
                childAlignment: UserInterfaceAlignment.RightDown,
                new GridUserInterfaceComponent(
                            name: "column",
                            spacing: new Vector2(8, 8),
                            columns: 3,
                            rows: 2,
                            position: new Vector2(0, 0),
                            size: new Vector2(64, 64),
                            childAlignment: UserInterfaceAlignment.RightDown,
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

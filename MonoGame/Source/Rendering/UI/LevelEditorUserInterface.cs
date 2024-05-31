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
            new SizedBoxUserInterfaceComponent(
                size: new Vector2(100, 100),
                localPosition: new Vector2(0, 0),
                child: new AlignmentUserInterfaceComponent(
                    alignment: UserInterfaceAlignment.Center,
                    child: new ContainerUserInterfaceComponent(
                        localPosition: new Vector2(10, 10),
                        child: new PaddingUserInterfaceComponent(
                            localPosition: new Vector2(0, 0),
                            paddingLeft: 5,
                            paddingRight: 5,
                            paddingTop: 5,
                            paddingBottom: 5,
                            child: new DirectionalListUserInterfaceComponent(
                                name: "list",
                                spacing: 0,
                                localPosition: new Vector2(0, 0),
                                direction: ListDirection.Vertical,
                                children: new List<IUserInterfaceComponent>
                                {
                                    new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0)),
                                    new SizedBoxUserInterfaceComponent(localPosition: new Vector2(0, 0), size: new Vector2(0, 10), child: null),
                                    new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0)),
                                    new SizedBoxUserInterfaceComponent(localPosition: new Vector2(0, 0), size: new Vector2(0, 10), child: null),
                                    new DirectionalListUserInterfaceComponent(
                                        name: "list",
                                        spacing: 0,
                                        localPosition: new Vector2(0, 0),
                                        direction: ListDirection.Horizontal,
                                        children: new List<IUserInterfaceComponent>
                                        {
                                            new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0)),
                                            new SizedBoxUserInterfaceComponent(localPosition: new Vector2(0, 0), size: new Vector2(10, 0), child: null),
                                            new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0))
                                        }
                                    )
                                }
                            )
                        )
                    )
                )
            )
        );
    }
}

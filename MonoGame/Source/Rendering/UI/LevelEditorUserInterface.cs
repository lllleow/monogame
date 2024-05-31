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
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.RightCenter,
                child: new ContainerUserInterfaceComponent(
                    localPosition: new Vector2(0, 0),
                    child: new PaddingUserInterfaceComponent(
                        localPosition: new Vector2(0, 0),
                        paddingLeft: 0,
                        paddingRight: 0,
                        paddingTop: 0,
                        paddingBottom: 0,
                        child: new DirectionalListUserInterfaceComponent(
                            name: "list",
                            spacing: 0,
                            localPosition: new Vector2(0, 0),
                            direction: ListDirection.Vertical,
                            children: new List<IUserInterfaceComponent>
                            {
                                new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0)),
                                new SizedBoxUserInterfaceComponent(localPosition: new Vector2(0, 0), size: new Vector2(0, 0)),
                                new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0)),
                                new SizedBoxUserInterfaceComponent(localPosition: new Vector2(0, 0), size: new Vector2(0, 0)),
                                new DirectionalListUserInterfaceComponent(
                                    name: "list",
                                    spacing: 0,
                                    localPosition: new Vector2(0, 0),
                                    direction: ListDirection.Horizontal,
                                    children: new List<IUserInterfaceComponent>
                                    {
                                        new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0)),
                                        new SizedBoxUserInterfaceComponent(localPosition: new Vector2(0, 0), size: new Vector2(0, 0)),
                                        new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0))
                                    }
                                )
                            }
                        )
                    )
                )
            )
        );
    }
}

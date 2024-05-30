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
                localPosition: new Vector2(0, 0),
                child: new PaddingUserInterfaceComponent(
                    localPosition: new Vector2(0, 0),
                    paddingLeft: 10,
                    paddingRight: 10,
                    paddingTop: 10,
                    paddingBottom: 10,
                    child: new DirectionalListUserInterfaceComponent(
                        name: "list",
                        spacing: 0,
                        localPosition: new Vector2(0, 0),
                        direction: ListDirection.Horizontal,
                        children: new List<IUserInterfaceComponent>
                        {
                            new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0)),
                            new TileSlotComponent("tile_slot", localPosition: new Vector2(0, 0))
                        }
                    )
                )
            )
        );
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Scripts;

namespace MonoGame;

public class LevelEditorUserInterface : UserInterface
{
    public LevelEditorUserInterface()
    {
        Name = "level_editor_user_interface";
        AddComponent(
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.CenterDown,
                child: new PaddingUserInterfaceComponent(
                    paddingLeft: 0,
                    paddingRight: 0,
                    paddingTop: 0,
                    paddingBottom: 4,
                    child: new DirectionalListUserInterfaceComponent(
                        name: "list",
                        spacing: 2,
                        localPosition: new Vector2(0, 0),
                        direction: ListDirection.Horizontal,
                        children: TileRegistry.Tiles.Keys.Select(tile => (IUserInterfaceComponent)new TileSlotComponent("tile_slot", tile: TileRegistry.GetTile(tile), localPosition: new Vector2(0, 0))).ToList()
                    )
                )
            )
        );
    }
}

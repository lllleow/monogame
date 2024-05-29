using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class LevelEditorUserInterface : UserInterface
{
    public LevelEditorUserInterface()
    {
        Name = "Level Editor";

        AddComponent(
            new DirectionalListUserInterfaceComponent(
                "row_1",
                ListDirection.Horizontal,
                null,
                new List<IUserInterfaceComponent> { new TileSlotComponent("slot_1", new Rectangle(0, 0, 16, 16)), new TileSlotComponent("slot_1", new Rectangle(0, 0, 16, 16)), new TileSlotComponent("slot_1", new Rectangle(0, 0, 16, 16)), new TileSlotComponent("slot_1", new Rectangle(0, 0, 16, 16)) }
            )
        );
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}

using System;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class LevelEditorUserInterfaceSlotComponentController : SlotUserInterfaceComponentController
{
    public override void OnGrab(SlotComponent slot)
    {
        base.OnGrab(slot);
    }

    public override void OnDrop(SlotComponent sourceSlot, SlotComponent destinationSlot)
    {
        base.OnDrop(sourceSlot, destinationSlot);
        if (destinationSlot is TileSlotComponent destinationTileSlot)
        {
            if (sourceSlot is TileSlotComponent sourceTileSlot)
            {
                destinationTileSlot.SetTile(sourceTileSlot.Tile);
            }
        }
    }
}

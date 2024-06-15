using System;
using System.Collections.Generic;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class SlotUserInterfaceComponentController
{
    public List<SlotComponent> Slots { get; set; } = new();
    private SlotComponent SourceSlot { get; set; }
    private SlotComponent DestinationSlot { get; set; }

    public void AddSlot(SlotComponent slot)
    {
        slot.OnSlotDragDrop = OnDrop;
        slot.OnSlotDragGrab = OnGrab;
        Slots.Add(slot);
    }

    public void RemoveSlot(SlotComponent slot)
    {
        Slots.Remove(slot);
    }

    public void OnGrab(SlotComponent slot)
    {
        SourceSlot = slot;
    }

    public void OnDrop(SlotComponent slot)
    {
        DestinationSlot = slot;
    }
}

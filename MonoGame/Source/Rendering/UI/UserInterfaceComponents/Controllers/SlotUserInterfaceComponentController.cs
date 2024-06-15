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

    public virtual void OnGrab(SlotComponent slot)
    {
        SourceSlot = slot;
    }

    private void OnDrop(SlotComponent slot)
    {
        DestinationSlot = slot;
        OnDrop(SourceSlot, DestinationSlot);
    }

    public virtual void OnDrop(SlotComponent sourceSlot, SlotComponent destinationSlot)
    {
    }
}

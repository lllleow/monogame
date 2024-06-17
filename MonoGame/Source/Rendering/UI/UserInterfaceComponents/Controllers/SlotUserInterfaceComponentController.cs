using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using MonoGame.Source.Utils.Helpers;
using MonoGame.Source.Utils.Loaders;
using MonoGame_Common;

namespace MonoGame;

public class SlotUserInterfaceComponentController
{
    public List<SlotComponent> Slots { get; set; } = new();
    private SlotComponent SourceSlot { get; set; }
    private SlotComponent DestinationSlot { get; set; }

    public SlotUserInterfaceComponentController()
    {
        InputEventManager.Subscribe(InputEventChannel.LevelEditor, inputEvent =>
        {
            if (inputEvent.EventType == InputEventType.MouseButtonUp)
            {
                ResetSlots();
            }
        });
    }

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
        ResetSlots();
    }

    public void ResetSlots()
    {

        SourceSlot = null;
        DestinationSlot = null;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (SourceSlot == null) return;
        var CurrentMouseState = Mouse.GetState();
        var worldPosition = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        var screenPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.UserInterfaceHandler.GetUITransform()));
        Vector2 iconSize = SourceSlot.GetPreferredSize() * 0.5f;
        spriteBatch.Draw(
                SpritesheetLoader.GetSpritesheet(SourceSlot.GetDrawable().Spritesheet),
                new Rectangle((int)screenPosition.X, (int)screenPosition.Y, (int)iconSize.X, (int)iconSize.Y),
                RectangleHelper.GetTextureRectangleFromCoordinates(0, 0),
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f);
    }
}

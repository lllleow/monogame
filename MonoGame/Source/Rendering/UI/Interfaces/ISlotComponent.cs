using System;
using MonoGame_Common.Util.Helpers;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface ISlotComponent : IUserInterfaceComponent
{
    public Action<SlotComponent> OnSlotDragGrab { get; set; }
    public Action<SlotComponent> OnSlotDragDrop { get; set; }
    public TextureLocation GetDrawable();
}
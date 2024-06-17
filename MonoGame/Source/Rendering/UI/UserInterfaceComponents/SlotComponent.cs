using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame_Common.Util.Helpers;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Utils.Helpers;
using MonoGame.Source.Utils.Loaders;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class SlotComponent : UserInterfaceComponent, ISlotComponent
{
    public SlotComponent(SlotUserInterfaceComponentController controller, string name, Vector2 localPosition) : base(name, localPosition)
    {
        Controller = controller;
        IsClickable = true;
    }

    public TextureLocation SlotTexture { get; set; } = TextureLocation.FirstTextureCoordinate("textures/slot");
    public bool IsSelected { get; set; } = false;
    public bool IsDragging { get; set; } = false;
    public Action<SlotComponent> OnSlotDragGrab { get; set; } = (slot) => { };
    public Action<SlotComponent> OnSlotDragDrop { get; set; } = (slot) => { };
    public SlotUserInterfaceComponentController Controller { get; set; }

    public override void Initialize(IUserInterfaceComponent parent)
    {
        base.Initialize(parent);

        if (Controller != null)
        {
            Controller.AddSlot(this);
        }

        AddInputSubscriber(InputEventManager.Subscribe(InputEventChannel.UI, inputEvent =>
        {
            if (inputEvent.EventType == InputEventType.MouseButtonDown)
            {
                if (inputEvent.Button == MouseButton.Left && MouseIntersectsComponent())
                {
                    OnSlotDragGrab?.Invoke(this);
                    IsDragging = true;
                }
            }
            else if (inputEvent.EventType == InputEventType.MouseButtonUp)
            {
                if (inputEvent.Button == MouseButton.Left)
                {
                    IsDragging = false;

                    if (MouseIntersectsComponent())
                    {
                        OnSlotDragDrop?.Invoke(this);
                    }
                }
            }
        }));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Enabled) return;
        base.Draw(spriteBatch);

        var textureLocation = GetDrawable();

        var position = GetPositionRelativeToParent();
        var size = GetPreferredSize();

        var textureRectangle = IsSelected
            ? RectangleHelper.GetTextureRectangleFromCoordinates(1, 0)
            : RectangleHelper.GetTextureRectangleFromCoordinates(0, 0);
        SlotTexture.TextureRectangle = RectangleHelper.ConvertToDrawingRectangle(textureRectangle);
        spriteBatch.Draw(
            SpritesheetLoader.GetSpritesheet(SlotTexture.Spritesheet),
            new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
            textureRectangle,
            Color.White * Opacity,
            0f,
            Vector2.Zero,
            SpriteEffects.None,
            1f);
        var iconSize = size * 0.5f;

        if (textureLocation != null)
        {
            var iconPosition = new Vector2(
                position.X + ((size.X / 2) - (iconSize.X / 2)),
                position.Y + ((size.Y / 2) - (iconSize.Y / 2)));
            spriteBatch.Draw(
                SpritesheetLoader.GetSpritesheet(textureLocation.Spritesheet),
                new Rectangle((int)iconPosition.X, (int)iconPosition.Y, (int)iconSize.X, (int)iconSize.Y),
                RectangleHelper.GetTextureRectangleFromCoordinates(0, 0),
                Color.White * Opacity,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                1f);
        }
    }

    public virtual TextureLocation GetDrawable()
    {
        throw new NotImplementedException();
    }

    public override Vector2 GetPreferredSize()
    {
        Vector2 size = new Vector2(16, 16);
        CalculatedSize = size;
        return size;
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class LabelUserInterfaceComponent : UserInterfaceComponent
{
    public string Text { get; set; } = string.Empty;
    public float Scale { get; set; } = 0.3f;

    public LabelUserInterfaceComponent(string text, Vector2 localPosition) : base("label", localPosition)
    {
        Text = text;
    }

    public override void Draw(SpriteBatch batch)
    {
        base.Draw(batch);
        Globals.spriteBatch.DrawString(Globals.defaultFont, Text, GetPositionRelativeToParent(), Color.White, scale: Scale, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 0f);
    }

    public override Vector2 GetPreferredSize()
    {
        return Globals.defaultFont.MeasureString(Text) * Scale;
    }
}

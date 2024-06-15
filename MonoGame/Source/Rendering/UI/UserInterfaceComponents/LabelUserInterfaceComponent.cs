using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class LabelUserInterfaceComponent : UserInterfaceComponent
{
    public LabelUserInterfaceComponent(string text, Vector2 localPosition) : base("label", localPosition)
    {
        Text = text;
    }

    public string Text { get; set; } = string.Empty;
    public float Scale { get; set; } = 0.35f;

    public override void Draw(SpriteBatch batch)
    {
        if (!Enabled) return;
        base.Draw(batch);
        Globals.SpriteBatch.DrawString(Globals.DefaultFont, Text, GetPositionRelativeToParent(), Color.White * Opacity, scale: Scale, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 1f);
    }

    public override Vector2 GetPreferredSize()
    {
        return Globals.DefaultFont.MeasureString(Text) * Scale;
    }
}
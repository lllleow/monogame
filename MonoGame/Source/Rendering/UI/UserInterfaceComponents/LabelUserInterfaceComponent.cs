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
    public float Scale { get; set; } = 0.3f;

    public override void Draw(SpriteBatch batch)
    {
        base.Draw(batch);
        Globals.SpriteBatch.DrawString(Globals.DefaultFont, Text, GetPositionRelativeToParent(), Color.White, scale: Scale, rotation: 0f, origin: Vector2.Zero, effects: SpriteEffects.None, layerDepth: 0f);
    }

    public override Vector2 GetPreferredSize()
    {
        return Globals.DefaultFont.MeasureString(Text) * Scale;
    }
}
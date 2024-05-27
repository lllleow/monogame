using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components;

namespace MonoGame;

public class BoundingBoxComponent : EntityComponent
{
    public Vector2 Size;

    public BoundingBoxComponent(Vector2 size)
    {
        Size = size;
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y, (int)Size.X, (int)Size.Y);
    }

    PrimitiveBatch primitiveBatch = new PrimitiveBatch(Globals.graphicsDevice.GraphicsDevice);
    public override void Draw(SpriteBatch spriteBatch)
    {
        Globals.spriteBatch.End();
        // Globals.graphicsDevice.GraphicsDevice.DepthStencilState = DepthStencilState.None;
        primitiveBatch.Begin(PrimitiveType.LineList);

        Rectangle rectangle = GetRectangle();
        Vector2 topLeft = rectangle.Location.ToVector2();
        Vector2 topRight = new Vector2(rectangle.Right, rectangle.Top);
        Vector2 bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
        Vector2 bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);

        primitiveBatch.AddVertex(topLeft, Color.Red);
        primitiveBatch.AddVertex(topRight, Color.Red);

        primitiveBatch.AddVertex(topRight, Color.Red);
        primitiveBatch.AddVertex(bottomRight, Color.Red);

        primitiveBatch.AddVertex(bottomRight, Color.Red);
        primitiveBatch.AddVertex(bottomLeft, Color.Red);

        primitiveBatch.AddVertex(bottomLeft, Color.Red);
        primitiveBatch.AddVertex(topLeft, Color.Red);

        primitiveBatch.End();
        Globals.spriteBatch.Begin(transformMatrix: Globals.camera.Transform, sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
    }
}

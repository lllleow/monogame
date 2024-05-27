using System;
using Microsoft.Xna.Framework;
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
        return new Rectangle((int)Entity.Position.X, (int)Entity.Position.Y, (int)Size.X * Tile.PixelSizeX, (int)Size.Y * Tile.PixelSizeY);
    }
}

using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface ITile : IInitializable
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TextureName { get; set; }
    public Texture2D Texture { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }
}

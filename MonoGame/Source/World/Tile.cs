using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class Tile : ITile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TextureName { get; set; }
    public Texture2D Texture { get; set; }
    public int SizeX { get; set; } = 1;
    public int SizeY { get; set; } = 1;

    public Tile(string id, string name, string textureName)
    {
        Id = id;
        Name = name;
        TextureName = textureName;
    }

    public Tile()
    {

    }

    public void Initialize()
    {
        Texture = Globals.contentManager.Load<Texture2D>(TextureName);
    }
}

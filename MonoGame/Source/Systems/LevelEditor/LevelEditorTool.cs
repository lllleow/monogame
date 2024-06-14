using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public abstract class LevelEditorTool
{
    public string Name { get; set; } = "Tool";
    public bool Enabled { get; set; } = false;
    public abstract void Initialize();
    public abstract void Update();
    public abstract void Draw(SpriteBatch spriteBatch);
}

using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class TileManipulatorEditorTool : LevelEditorTool
{
    public string SelectedTile { get; set; } = "base.grass";
    public (int PosX, int PosY) CursorPosition { get; set; } = (0, 0);

    public override void Draw(SpriteBatch spriteBatch)
    {
    }

    public override void Initialize()
    {
    }

    public override void Update()
    {
    }

    public void SetSelectedTile(string tileId)
    {
        SelectedTile = tileId;
    }

    public void SetCursorPosition(int x, int y)
    {
        CursorPosition = (x, y);
    }
}

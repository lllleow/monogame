using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame.Source;

namespace MonoGame;

public class SelectLevelEditorTool : TileManipulatorEditorTool
{
    public bool IsPlacing { get; set; } = false;
    public (int PosX, int PosY)? StartingPosition { get; set; } = null;
    public (int PosX, int PosY)? EndPosition { get; set; } = null;
    private Texture2D tileCursor;

    public SelectLevelEditorTool()
    {
        Name = "Select";
        tileCursor = Globals.ContentManager.Load<Texture2D>("textures/tile_cursor");

        ToolConfigurations.Add(new SelectFillToolConfiguration());
    }

    public override void Initialize()
    {
        InputEventManager.Subscribe(inputEvent =>
        {
            if (!Enabled) return;

            if (inputEvent.EventType == InputEventType.MouseButtonDown)
            {
                StartingPosition = CursorPosition;
            }
        });
    }

    public override void Update()
    {
        if (!Enabled) return;
        base.Update();
        MouseState currentMouseState = Mouse.GetState();
        if (currentMouseState.LeftButton == ButtonState.Released)
        {
            if (StartingPosition != null && EndPosition != null)
            {
                OnSelectTiles();
            }
        }
        else
        {
            EndPosition = CursorPosition;
        }
    }

    public void ClearSelection()
    {
        EndPosition = null;
        StartingPosition = null;
    }

    public virtual void OnSelectTiles()
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Enabled) return;

        if (StartingPosition == null || EndPosition == null)
        {
            return;
        }

        int startX = Math.Min(StartingPosition?.PosX ?? 0, EndPosition?.PosX ?? 0);
        int startY = Math.Min(StartingPosition?.PosY ?? 0, EndPosition?.PosY ?? 0);
        int endX = Math.Max(StartingPosition?.PosX ?? 0, EndPosition?.PosX ?? 0);
        int endY = Math.Max(StartingPosition?.PosY ?? 0, EndPosition?.PosY ?? 0);

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (x == startX || x == endX || y == startY || y == endY)
                {
                    Rectangle destinationRectangle = new Rectangle(x * SharedGlobals.PixelSizeX, y * SharedGlobals.PixelSizeY, 16, 16);
                    spriteBatch.End();
                    Globals.DefaultSpriteBatchBegin();
                    spriteBatch.Draw(tileCursor, destinationRectangle, Color.White);
                }
            }
        }
    }
}

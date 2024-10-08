﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.Player;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents.Custom;
using MonoGame.Source.WorldNamespace;

namespace MonoGame.Source.Rendering.UI.UserInterfaces;

public class LevelEditorUserInterface : UserInterface
{
    private MouseState currentMouseState;
    private Texture2D tileCursor;
    private (int PosX, int PosY) cursorPosition;
    public string SelectedTile { get; set; } = "base.grass";
    public LevelEditorToolBarUserInterfaceComponent ToolBar { get; set; } = new();

    public LevelEditorUserInterface()
    {
        Name = "level_editor_user_interface";
        AddComponent(
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.CenterDown,
                child: new PaddingUserInterfaceComponent(
                    0,
                    0,
                    0,
                    8,
                    new HotbarUserInterfaceComponent(
                        new Vector2(0, 0),
                        (tile) =>
                        {
                            SelectedTile = tile;
                        }
                    )
                )
            )
        );

        AddComponent(
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.LeftCenter,
                child: new PaddingUserInterfaceComponent(
                    8,
                    0,
                    0,
                    0,
                    ToolBar
                )
            )
        );
    }

    public override void Initialize()
    {
        base.Initialize();
        tileCursor = Globals.ContentManager.Load<Texture2D>("textures/tile_cursor");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        DrawTileCursor(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        ToolBar.CursorPosition = cursorPosition;
        ToolBar.SelectedTile = SelectedTile;
    }

    public void DrawTileCursor(SpriteBatch spriteBatch)
    {
        currentMouseState = Mouse.GetState();
        Vector2 screenPosition = new Vector2(currentMouseState.X, currentMouseState.Y);
        (int PosX, int PosY) globalPosition = World.GetGlobalPositionFromScreenPosition(screenPosition);
        cursorPosition = globalPosition;
        Rectangle destinationRectangle = new Rectangle(globalPosition.PosX * SharedGlobals.PixelSizeX, globalPosition.PosY * SharedGlobals.PixelSizeY, 16, 16);

        spriteBatch.End();
        Globals.DefaultSpriteBatchBegin();
        spriteBatch.Draw(tileCursor, destinationRectangle, Color.White);
    }

    public override bool IsVisible()
    {
        return Globals.World.GetLocalPlayer()?.GameMode == GameMode.LevelEditor;
    }
}
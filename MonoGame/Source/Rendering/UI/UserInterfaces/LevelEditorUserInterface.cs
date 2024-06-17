using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame.Source.GameModes;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents.Custom;
using MonoGame.Source.WorldNamespace;
using System.ComponentModel;
using MonoGame.Source.Rendering.UI.Interfaces;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace MonoGame.Source.Rendering.UI.UserInterfaces;

public class LevelEditorUserInterface : UserInterface
{
    private MouseState currentMouseState;
    private Texture2D tileCursor;
    private (int PosX, int PosY) cursorPosition;
    public string SelectedTile { get; set; } = "base.grass";
    public LevelEditorToolBarUserInterfaceComponent ToolBar { get; set; } = new();
    private TileSelectorUserInterfaceComponent tileSelectorComponent;
    public SlotUserInterfaceComponentController SlotController { get; set; }
    public LabelUserInterfaceComponent PosXLabel { get; set; }
    public LabelUserInterfaceComponent PosYLabel { get; set; }

    public LevelEditorUserInterface()
    {
        Name = "level_editor_user_interface";

        SlotController = new LevelEditorUserInterfaceSlotComponentController();
        tileSelectorComponent = new TileSelectorUserInterfaceComponent(SlotController);

        AddComponent(
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.CenterDown,
                child: new PaddingUserInterfaceComponent(
                    0,
                    0,
                    0,
                    8,
                    new HotbarUserInterfaceComponent(
                        SlotController,
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

        AddComponent(
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.RightDown,
                child: new PaddingUserInterfaceComponent(
                    8,
                    0,
                    0,
                    0,
                    new LevelEditorZoomSelectorUserInterfaceComponent()
                )
            )
        );

        AddComponent(
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.Center,
                child: tileSelectorComponent
            )
        );

        PosXLabel = new LabelUserInterfaceComponent("Pos X: ", new Vector2(0, 0));
        PosYLabel = new LabelUserInterfaceComponent("Pos Y: ", new Vector2(0, 0));
        AddComponent(
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.LeftDown,
                child: new PaddingUserInterfaceComponent(
                    8,
                    8,
                    8,
                    8,
                    child: new ContainerUserInterfaceComponent(
                        new Vector2(0, 0),
                        child: new PaddingUserInterfaceComponent(
                            4,
                            4,
                            4,
                            4,
                            child: new SizedBoxUserInterfaceComponent(
                                new Vector2(0, 0),
                                new Vector2(25, -1),
                                child: new StretchUserInterfaceComponent(
                                    Axis.Horizontal,
                                    child: new DirectionalListUserInterfaceComponent(
                                        "list",
                                        Axis.Vertical,
                                        localPosition: new Vector2(0, 0),
                                        spacing: 2,
                                        children: new List<IUserInterfaceComponent>()
                                        {
                                            PosXLabel,
                                            PosYLabel,
                                        }
                                    )
                                )
                            )
                        )
                    )
                    {
                        BackgroundImage = "textures/ui_background",
                        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile
                    }
                )
            )
        );
    }

    public override void Initialize()
    {
        base.Initialize();
        tileCursor = Globals.ContentManager.Load<Texture2D>("textures/tile_cursor");

        InputEventManager.Subscribe(InputEventChannel.LevelEditor, inputEvent =>
        {
            if (inputEvent.EventType == InputEventType.KeyUp)
            {
                if (inputEvent.Key == Microsoft.Xna.Framework.Input.Keys.E)
                {
                    tileSelectorComponent.Enabled = !tileSelectorComponent.Enabled;
                    var gameModeController = Globals.World.GetGameModeController<LevelEditorGameModeController>();
                    if (gameModeController != null)
                    {
                        gameModeController.ShowCursor = !tileSelectorComponent.Enabled;
                        gameModeController.BlockMovement = tileSelectorComponent.Enabled;
                    }
                }
            }
        });
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
        if (!Globals.World.GetGameModeController<LevelEditorGameModeController>()?.ShowCursor ?? true)
        {
            return;
        }

        currentMouseState = Mouse.GetState();
        Vector2 screenPosition = new Vector2(currentMouseState.X, currentMouseState.Y);
        (int PosX, int PosY) globalPosition = World.GetGlobalPositionFromScreenPosition(screenPosition);
        PosXLabel.Text = "Pos X: " + globalPosition.PosX;
        PosYLabel.Text = "Pos Y: " + globalPosition.PosY;
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
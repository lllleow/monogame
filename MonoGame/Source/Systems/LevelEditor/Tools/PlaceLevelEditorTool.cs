using System;
using Microsoft.Xna.Framework;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.Player;
using MonoGame.Source.Multiplayer;

namespace MonoGame;

public class PlaceLevelEditorTool : TileManipulatorEditorTool
{
    public PlaceLevelEditorTool()
    {
        Name = "Place Tool";
    }

    public override void Initialize()
    {
        InputEventManager.Subscribe(inputEvent =>
        {
            if (inputEvent.EventType == InputEventType.MouseButtonUp && Enabled)
            {
                NetworkClient.SendMessage(new RequestToPlaceTileNetworkMessage()
                {
                    TileId = SelectedTile,
                    Layer = TileDrawLayer.Tiles,
                    PosX = CursorPosition.PosX,
                    PosY = CursorPosition.PosY
                });
            }
        });
    }
}

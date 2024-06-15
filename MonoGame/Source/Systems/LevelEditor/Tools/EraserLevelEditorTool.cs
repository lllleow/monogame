﻿using System;
using MonoGame_Common.Enums;
using MonoGame.Source.Multiplayer;

namespace MonoGame;

public class EraserLevelEditorTool : SelectLevelEditorTool
{
    public EraserLevelEditorTool()
    {
        Name = "Eraser";
    }

    public override void OnSelectTiles()
    {
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
                    NetworkClient.SendMessage(new RequestToDeleteTileNetworkMessage()
                    {
                        TileId = SelectedTile,
                        Layer = TileDrawLayer.Tiles,
                        PosX = x,
                        PosY = y
                    });
                }
            }
        }

        ClearSelection();
    }
}

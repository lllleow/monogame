using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.Player;
using MonoGame.Source;
using MonoGame.Source.Multiplayer;

namespace MonoGame;

public class PlaceLevelEditorTool : SelectLevelEditorTool
{
    public PlaceLevelEditorTool() : base()
    {
        Name = "Place";
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
                if (GetToolConfiguration<SelectFillToolConfiguration>()?.Enabled ?? false)
                {
                    NetworkClient.SendMessage(new RequestToPlaceTileNetworkMessage()
                    {
                        TileId = SelectedTile,
                        Layer = TileDrawLayer.Tiles,
                        PosX = x,
                        PosY = y
                    });
                }
                else
                {
                    if (x == startX || x == endX || y == startY || y == endY)
                    {
                        NetworkClient.SendMessage(new RequestToPlaceTileNetworkMessage()
                        {
                            TileId = SelectedTile,
                            Layer = TileDrawLayer.Tiles,
                            PosX = x,
                            PosY = y
                        });
                    }
                }
            }
        }

        ClearSelection();
    }
}
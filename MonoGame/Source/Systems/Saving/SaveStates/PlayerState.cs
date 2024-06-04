using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class PlayerState
{
    public string UUID { get; set; }
    public Vector2 Position { get; set; }
    public string SelectedTile { get; set; }

    public PlayerState()
    {

    }

    public PlayerState(Player player)
    {
        SelectedTile = player.selectedTile;
        Position = player.Position;
        UUID = player.UUID;
    }
}

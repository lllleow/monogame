using System;
using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame;

public interface ITileComponent
{
    public Tile Tile { get; set; }
}

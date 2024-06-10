﻿using MonoGame.Source.Systems.Tiles.Utils;
using MonoGame_Common.Util.Enum;

namespace MonoGame.Source.Systems.Tiles.Interfaces;

public interface ITileTextureProcessor
{
    public (int TextureCoordinateX, int TextureCoordinateY) Process(TileNeighborConfiguration configuration);

    public bool CanConnect(TileNeighborConfiguration configuration, Direction direction);

    public bool IsOfSameType(TileNeighborConfiguration configuration, Direction direction);

    public bool IsWhitelisted(TileNeighborConfiguration configuration, Direction direction);
}
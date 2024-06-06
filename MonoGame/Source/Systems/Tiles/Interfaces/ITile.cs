﻿using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.Enum;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Tiles.Enums;
using MonoGame.Source.Util.Enum;

namespace MonoGame.Source.Systems.Tiles.Interfaces;

public interface ITile
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string SpritesheetName { get; set; }

    public int TextureX { get; set; }

    public int TextureY { get; set; }

    float Scale { get; set; }

    public float Opacity { get; set; }

    public int SizeX { get; set; }

    public int SizeY { get; set; }

    public int PixelOffsetX { get; set; }

    public int PixelOffsetY { get; set; }

    public ITileTextureProcessor TextureProcessor { get; set; }

    public CollisionMode CollisionMode { get; set; }

    public string CollisionMaskSpritesheetName { get; set; }

    public int WorldX { get; set; }

    public int WorldY { get; set; }

    public int LocalX { get; set; }

    public int LocalY { get; set; }

    public bool Walkable { get; set; }

    public (int TextureCoordinateX, int TextureCoordinateY) DefaultTextureCoordinates { get; set; }

    public string[] ConnectableTiles { get; set; }

    public void UpdateTextureCoordinates(TileDrawLayer layer);

    public void Initialize(int localX, int localY, int worldX, int worldY);

    void OnNeighborChanged(ITile neighbor, TileDrawLayer layer, Direction direction);

    public Rectangle GetSpriteRectangle()
    {
        return new Rectangle(TextureX * Tile.PixelSizeX, TextureY * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }

    public Rectangle GetRectangle()
    {
        return new Rectangle(WorldX, WorldY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }

    public TextureLocation GetTextureLocation()
    {
        return new TextureLocation(SpritesheetName, GetSpriteRectangle());
    }
}

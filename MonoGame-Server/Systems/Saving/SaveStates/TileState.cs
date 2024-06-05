using System;

namespace MonoGame;

public class TileState
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    public TileDrawLayer Layer { get; set; }

    public TileState()
    {
        
    }

    public TileState(TileDrawLayer layer, ITile tile)
    {
        Id = tile.Id;
        Name = tile.Name;
        LocalX = tile.LocalX;
        LocalY = tile.LocalY;
        Layer = layer;
    }
}

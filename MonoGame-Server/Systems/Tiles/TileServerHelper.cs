using MonoGame_Common.Enums;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Tile;
using MonoGame_Server.Systems.Server;

namespace MonoGame_Server;

public class TileServerHelper
{
    public static TileNeighborConfiguration GetNeighborConfiguration(CommonTile centerTile, TileDrawLayer layer, int worldX, int worldY)
    {
        var left = NetworkServer.Instance.ServerWorld.GetTileAtPosition(layer, worldX - 1, worldY)?.GetCommonTile();
        var right = NetworkServer.Instance.ServerWorld.GetTileAtPosition(layer, worldX + 1, worldY)?.GetCommonTile();
        var up = NetworkServer.Instance.ServerWorld.GetTileAtPosition(layer, worldX, worldY - 1)?.GetCommonTile();
        var down = NetworkServer.Instance.ServerWorld.GetTileAtPosition(layer, worldX, worldY + 1)?.GetCommonTile();

        var left_top = NetworkServer.Instance.ServerWorld.GetTileAtPosition(layer, worldX - 1, worldY - 1)?.GetCommonTile();
        var right_top = NetworkServer.Instance.ServerWorld.GetTileAtPosition(layer, worldX + 1, worldY - 1)?.GetCommonTile();
        var left_bottom = NetworkServer.Instance.ServerWorld.GetTileAtPosition(layer, worldX - 1, worldY + 1)?.GetCommonTile();
        var right_bottom = NetworkServer.Instance.ServerWorld.GetTileAtPosition(layer, worldX + 1, worldY + 1)?.GetCommonTile();

        return new TileNeighborConfiguration(centerTile, left, right, up, down, left_top, right_top, left_bottom, right_bottom);
    }
}

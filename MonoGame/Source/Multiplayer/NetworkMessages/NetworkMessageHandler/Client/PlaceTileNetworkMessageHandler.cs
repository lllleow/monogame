using System;
using System.Numerics;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.Systems.Chunks.Interfaces;

namespace MonoGame;

public class PlaceTileNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        PlaceTileNetworkMessage placeTileNetworkMessage = (PlaceTileNetworkMessage)message;
        Vector2 worldPosition = new Vector2(placeTileNetworkMessage.PosX, placeTileNetworkMessage.PosY);

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        IChunk chunk = Globals.world.CreateOrGetChunk(chunkX, chunkY);

        if (chunk.GetTile(TileDrawLayer.Tiles, localX, localY) != null)
        {
            chunk.DeleteTile(TileDrawLayer.Tiles, localX, localY);
        }
        else
        {
            chunk.SetTileAndUpdateNeighbors(placeTileNetworkMessage.TileId, TileDrawLayer.Tiles, localX, localY);
        }
    }
}

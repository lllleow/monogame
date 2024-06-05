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
        Globals.world.SetTileAtPosition(placeTileNetworkMessage.TileId, TileDrawLayer.Tiles, placeTileNetworkMessage.PosX, placeTileNetworkMessage.PosY);
    }
}

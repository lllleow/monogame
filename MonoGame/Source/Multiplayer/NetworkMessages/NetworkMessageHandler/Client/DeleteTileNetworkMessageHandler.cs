using System;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame;

public class DeleteTileNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        PlaceTileNetworkMessage placeTileNetworkMessage = (PlaceTileNetworkMessage)message;
        // Globals.world.DeleteTile(TileDrawLayer.Tiles, placeTileNetworkMessage.PosX, placeTileNetworkMessage.PosY);
    }
}

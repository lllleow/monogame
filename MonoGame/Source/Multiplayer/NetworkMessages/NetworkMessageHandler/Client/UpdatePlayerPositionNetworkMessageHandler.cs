﻿using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame;

public class UpdatePlayerPositionNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        UpdatePlayerPositionNetworkMessage updatePlayerPositionNetworkMessage = (UpdatePlayerPositionNetworkMessage)message;
        Globals.World.GetPlayerByUUID(updatePlayerPositionNetworkMessage.UUID).Position = updatePlayerPositionNetworkMessage.Position;
    }
}

using System;
using MonoGame.Source.Multiplayer.NetworkMessageHandler.Client;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;

namespace MonoGame;

public enum NetworkMessageTypes
{
    AuthenticateUserNetworkMessage = 0,
    AuthenticationResultNetworkMessage = 1,
    RequestToLoadWorldNetworkMessage = 2,
    ChunkDataNetworkMessage = 3,
    SpawnPlayerNetworkMessage = 4,
    RequestMovementNetworkMessage = 5,
    UpdatePlayerPositionNetworkMessage = 6,
    MovePlayerNetworkMessage = 7
}

public static class NetworkMessageTypeClientHelper
{
    public static Type GetTypeFromMessageType(NetworkMessageTypes messageType)
    {
        switch (messageType)
        {
            case NetworkMessageTypes.AuthenticateUserNetworkMessage:
                return typeof(AuthenticateUserNetworkMessage);
            case NetworkMessageTypes.AuthenticationResultNetworkMessage:
                return typeof(AuthenticationResultNetworkMessage);
            case NetworkMessageTypes.RequestToLoadWorldNetworkMessage:
                return typeof(RequestToLoadWorldNetworkMessage);
            case NetworkMessageTypes.ChunkDataNetworkMessage:
                return typeof(ChunkDataNetworkMessage);
            case NetworkMessageTypes.SpawnPlayerNetworkMessage:
                return typeof(SpawnPlayerNetworkMessage);
            case NetworkMessageTypes.RequestMovementNetworkMessage:
                return typeof(RequestMovementNetworkMessage);
            case NetworkMessageTypes.UpdatePlayerPositionNetworkMessage:
                return typeof(UpdatePlayerPositionNetworkMessage);
            case NetworkMessageTypes.MovePlayerNetworkMessage:
                return typeof(MovePlayerNetworkMessage);

        }

        return null;
    }

    public static Type GetHandlerForClientMessageType(NetworkMessageTypes messageTypes)
    {
        switch (messageTypes)
        {
            case NetworkMessageTypes.AuthenticationResultNetworkMessage:
                return typeof(AuthenticationResultNetworkMessageHandler);
            case NetworkMessageTypes.ChunkDataNetworkMessage:
                return typeof(ChunkDataNetworkMessageHandler);
            case NetworkMessageTypes.SpawnPlayerNetworkMessage:
                return typeof(SpawnPlayerNetworkMessageHandler);
            case NetworkMessageTypes.UpdatePlayerPositionNetworkMessage:
                return typeof(UpdatePlayerPositionNetworkMessageHandler);
            case NetworkMessageTypes.MovePlayerNetworkMessage:
                return typeof(MovePlayerNetworkMessageHandler);
        }

        return null;
    }
}

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
        }

        return null;
    }
}

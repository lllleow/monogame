using System;

namespace MonoGame;

public enum NetworkMessageTypes
{
    HandshakeMessage = 0,
    SpawnPlayerMessage = 1,
    ServerAbortClientConnectionMessage = 2,
    MovePlayerMessage = 3,
    KeyClickedEvent = 4
}

public static class NetworkMessageTypeHelper
{
    public static Type GetTypeFromMessageType(NetworkMessageTypes messageType)
    {
        switch (messageType)
        {
            case NetworkMessageTypes.HandshakeMessage:
                return typeof(HandshakeMessage);
            case NetworkMessageTypes.SpawnPlayerMessage:
                return typeof(SpawnPlayerMessage);
            case NetworkMessageTypes.ServerAbortClientConnectionMessage:
                return typeof(ServerAbortClientConnectionMessage);
            default:
                throw new ArgumentException("Invalid network message type");
        }
    }
}

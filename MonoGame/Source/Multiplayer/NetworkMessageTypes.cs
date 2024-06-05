using System;

namespace MonoGame;

public enum NetworkMessageTypes
{
    AuthenticateUserNetworkMessage = 0,
}

public static class NetworkMessageTypeHelper
{
    public static Type GetTypeFromMessageType(NetworkMessageTypes messageType)
    {
        switch (messageType)
        {
            case NetworkMessageTypes.AuthenticateUserNetworkMessage:
                return typeof(AuthenticateUserNetworkMessage);
            default:
                throw new ArgumentException("Invalid network message type");
        }
    }
}

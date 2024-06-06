using MonoGame;
using MonoGame_Server.Systems.Server.MessageHandlers;

namespace MonoGame_Server;

public class NetworkMessageTypeServerHelper
{
    public static Type GetHandlerForServerMessageType(NetworkMessageTypes messageType)
    {
        switch (messageType)
        {
            case NetworkMessageTypes.AuthenticateUserNetworkMessage:
                return typeof(AuthenticateUserServerMessageHandler);
            case NetworkMessageTypes.RequestToLoadWorldNetworkMessage:
                return typeof(RequestToLoadWorldServerMessageHandler);
            case NetworkMessageTypes.RequestMovementNetworkMessage:
                return typeof(RequestMovementNetworkMessageHandler);
            case NetworkMessageTypes.RequestToPlaceTileNetworkMessage:
                return typeof(RequestToPlaceTileNetworkMessageHandler);
            default:
                throw new ArgumentException("Invalid network message type");
        }
    }
}

using MonoGame_Server.Systems.Server.MessageHandlers;
using MonoGame.Source.Multiplayer;

namespace MonoGame_Server.Systems.Server;

public class NetworkMessageTypeServerHelper
{
    public static Type GetHandlerForServerMessageType(NetworkMessageTypes messageType)
    {
        return messageType switch
        {
            NetworkMessageTypes.AuthenticateUserNetworkMessage => typeof(AuthenticateUserServerMessageHandler),
            NetworkMessageTypes.RequestToLoadWorldNetworkMessage => typeof(RequestToLoadWorldServerMessageHandler),
            NetworkMessageTypes.RequestMovementNetworkMessage => typeof(RequestMovementNetworkMessageHandler),
            NetworkMessageTypes.RequestToPlaceTileNetworkMessage => typeof(RequestToPlaceTileNetworkMessageHandler),
            _ => throw new ArgumentException("Invalid network message type"),
        };
    }
}

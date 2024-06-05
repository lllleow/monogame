using MonoGame;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;
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
            default:
                throw new ArgumentException("Invalid network message type");
        }
    }
}

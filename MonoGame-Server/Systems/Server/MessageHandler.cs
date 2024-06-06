using LiteNetLib;
using LiteNetLib.Utils;
using MonoGame_Server.Systems.Server.MessageHandlers;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame_Server.Systems.Server;

public class MessageHandler
{
    public static void HandleMessage(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod, byte channel)
    {
        NetworkMessageTypes messageType = (NetworkMessageTypes)reader.GetByte();
        Type messageObjectType = NetworkMessageTypeClientHelper.GetTypeFromMessageType(messageType);

        var message = Activator.CreateInstance(messageObjectType);
        if (message != null)
        {
            ((INetworkMessage)message).Deserialize(reader);
        }

        Type handlerType = NetworkMessageTypeServerHelper.GetHandlerForServerMessageType(messageType);
        if (handlerType != null)
        {
            var handler = Activator.CreateInstance(handlerType);
            if (handler != null)
            {
                ((IServerMessageHandler)handler!).Execute(peer, channel, (INetworkMessage)message!);
            }
        }

        Console.WriteLine("Server received: " + message);
    }
}

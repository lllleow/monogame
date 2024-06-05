using LiteNetLib;
using LiteNetLib.Utils;
using MonoGame;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame_Server.Systems.Server.MessageHandlers;

namespace MonoGame_Server.Systems.Server;

public class MessageHandler
{
    public static void HandleMessage(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod, byte channel)
    {
        NetworkMessageTypes messageType = (NetworkMessageTypes)reader.GetByte();
        Type messageObjectType = NetworkMessageTypeClientHelper.GetTypeFromMessageType(messageType);

        INetworkMessage? message = (INetworkMessage)Activator.CreateInstance(messageObjectType);
        message?.Deserialize(reader);

        Type handlerType = NetworkMessageTypeServerHelper.GetHandlerForServerMessageType(messageType);
        if (handlerType != null)
        {
            IServerMessageHandler handler = (IServerMessageHandler)Activator.CreateInstance(handlerType);
            handler!.Execute(peer, channel, message!);
        }

        Console.WriteLine("Server received: " + message);
    }
}

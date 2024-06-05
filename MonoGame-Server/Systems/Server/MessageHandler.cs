using LiteNetLib;
using LiteNetLib.Utils;
using MonoGame;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame_Server.Systems.Server;

public class MessageHandler
{
    public static void HandleMessage(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod, byte channel)
    {
        Type messageType = NetworkMessageTypeHelper.GetTypeFromMessageType((NetworkMessageTypes)reader.GetByte());
        INetworkMessage? message = (INetworkMessage?)Activator.CreateInstance(messageType);
        message?.Deserialize(reader);

        Console.WriteLine("Server received: " + message?.Serialize().ToString());
    }
}

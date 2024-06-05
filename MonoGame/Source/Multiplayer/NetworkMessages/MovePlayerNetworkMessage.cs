using System;
using System.Numerics;
using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame;

public class MovePlayerNetworkMessage : INetworkMessage
{
    public Direction Direction;
    public Vector2 Displacement;

    public NetDataReader Deserialize(NetDataReader reader)
    {
        Direction = (Direction)reader.GetByte();
        Displacement = new Vector2(reader.GetFloat(), reader.GetFloat());
        return reader;
    }

    public int GetMessageTypeId()
    {
        return (int)NetworkMessageTypes.MovePlayerMessage;
    }

    public NetDataWriter Serialize()
    {
        NetDataWriter writer = new NetDataWriter();
        writer.Put((byte)Direction);
        writer.Put(Displacement.X);
        writer.Put(Displacement.Y);
        return writer;
    }
}

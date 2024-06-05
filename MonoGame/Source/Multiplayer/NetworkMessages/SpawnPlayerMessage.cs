using System;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame;

public class SpawnPlayerMessage : NetworkMessage, IClientExecutableMessage
{
    public string UUID;
    public Vector2 Position;

    public SpawnPlayerMessage()
    {
    }

    public SpawnPlayerMessage(string uuid, Vector2 position)
    {
        UUID = uuid;
        Position = position;
    }

    public override NetDataReader Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        Position = new Vector2(reader.GetFloat(), reader.GetFloat());
        return reader;
    }

    public override NetDataWriter Serialize()
    {
        NetDataWriter data = new NetDataWriter();

        data.Put((byte)GetMessageTypeId());
        data.Put(UUID);
        data.Put(Position.X);
        data.Put(Position.Y);

        return data;
    }

    public override int GetMessageTypeId()
    {
        return (int)NetworkMessageTypes.SpawnPlayerMessage;
    }

    public void ExecuteOnClient()
    {
        Player player = new Player(Position);
        player.UUID = UUID;
        player.Position = Position;

        Globals.world.Players.Add(player);
    }
}

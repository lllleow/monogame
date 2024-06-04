using System;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class HandshakeMessage : NetworkMessage, IServerExecutableMessage
{
    public string UUID { get; set; }

    public HandshakeMessage()
    {
    }

    public HandshakeMessage(string uuid)
    {
        UUID = uuid;
    }

    public override NetDataWriter Serialize()
    {
        NetDataWriter data = new NetDataWriter();

        data.Put((byte)GetMessageTypeId());
        data.Put(UUID);

        return data;
    }

    public override NetDataReader Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        return reader;
    }

    public override int GetMessageTypeId()
    {
        return (int)NetworkMessageTypes.HandshakeMessage;
    }

    public void ExecuteOnServer(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod, byte channel)
    {
        if (Globals.world.Players.Any(player => player.UUID == UUID))
        {
            Globals.GetNetworkManager().GetServer().SendMessage(UUID, new ServerAbortClientConnectionMessage("Player with UUID already connected"));
            return;
        }

        if (Globals.world.Players.Any(player => player.UUID == UUID))
        {
            Globals.GetNetworkManager().GetServer().SendMessage(UUID, new ServerAbortClientConnectionMessage("A connection for this UUID already exists"));
            return;
        }

        Globals.GetNetworkManager().GetServer().RegisterConnection(UUID, peer);

        Player player = Globals.world.GetPlayerByUUID(UUID);
        Globals.GetNetworkManager().GetServer().BroadcastMessage(new SpawnPlayerMessage(UUID, player?.Position ?? Globals.spawnPosition));

        foreach (Player otherPlayer in Globals.world.Players)
        {
            Globals.GetNetworkManager().GetServer().SendMessage(UUID, new SpawnPlayerMessage(otherPlayer.UUID, otherPlayer.Position));
        }
    }
}

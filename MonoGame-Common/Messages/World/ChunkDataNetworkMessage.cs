using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using MonoGame_Common.States;

namespace MonoGame_Common.Messages.World;

[NetworkMessage(4)]
public class ChunkDataNetworkMessage : NetworkMessage
{
    public ChunkDataNetworkMessage()
    {
    }

    public ChunkDataNetworkMessage(ChunkState chunkState)
    {
        ChunkState = chunkState;
    }

    public ChunkState ChunkState { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        ChunkState = new ChunkState();
        ChunkState.Deserialize(reader);
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(ChunkState);
        return data;
    }
}
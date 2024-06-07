using LiteNetLib.Utils;
using MonoGame.Source.WorldNamespace.WorldStates;

namespace MonoGame.Source.Multiplayer.Messages.World
{
    [NetworkMessage(4)]
    public class ChunkDataNetworkMessage : NetworkMessage
    {
        public ChunkState ChunkState { get; set; }

        public ChunkDataNetworkMessage()
        {
        }

        public ChunkDataNetworkMessage(ChunkState chunkState)
        {
            ChunkState = chunkState;
        }

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
}
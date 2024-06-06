using LiteNetLib.Utils;
using MonoGame.Source.WorldNamespace.WorldStates;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server
{
    public class ChunkDataNetworkMessage : NetworkMessage
    {
        public ChunkState ChunkState;

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
            data.Put((byte)NetworkMessageTypes.ChunkDataNetworkMessage);
            data.Put(ChunkState);
            return data;
        }
    }
}
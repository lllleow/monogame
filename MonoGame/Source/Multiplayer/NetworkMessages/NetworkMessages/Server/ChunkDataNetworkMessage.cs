using System;
using System.Collections.Generic;
using LiteNetLib.Utils;

namespace MonoGame
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
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.ChunkDataNetworkMessage);
            data.Put(ChunkState);
            return data;
        }
    }
}
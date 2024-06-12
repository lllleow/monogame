﻿using LiteNetLib.Utils;

namespace MonoGame_Common.States.TileComponents;

public class TileComponentState : INetSerializable
{
    public virtual void Deserialize(NetDataReader reader)
    {
        throw new NotImplementedException();
    }

    public virtual void Serialize(NetDataWriter writer)
    {
        throw new NotImplementedException();
    }
}

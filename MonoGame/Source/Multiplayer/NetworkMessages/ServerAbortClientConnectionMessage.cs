using System;
using LiteNetLib.Utils;

namespace MonoGame;

public class ServerAbortClientConnectionMessage : NetworkMessage
{
    public string Reason { get; set; }

    public ServerAbortClientConnectionMessage(string reason) : base()
    {
        Reason = reason;
    }

    public override NetDataWriter Serialize()
    {
        NetDataWriter data = new NetDataWriter();

        data.Put((byte)GetMessageTypeId());
        data.Put(Reason);

        return data;
    }

    public override NetDataReader Deserialize(NetDataReader reader)
    {
        Reason = reader.GetString();
        return reader;
    }

    public override int GetMessageTypeId()
    {
        return (int)NetworkMessageTypes.ServerAbortClientConnectionMessage;
    }
}

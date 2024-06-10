using LiteNetLib.Utils;
using MonoGame_Common.Attributes;

namespace MonoGame_Common.Messages.Components;

[NetworkMessage(16)]
public class RegisterEntityComponentNetworkMessage : NetworkMessage
{
    public RegisterEntityComponentNetworkMessage()
    {
    }

    public RegisterEntityComponentNetworkMessage(string uuid, Type componentType)
    {
        UUID = uuid;
        ComponentType = componentType;
    }

    public string UUID { get; set; }
    public Type ComponentType { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        var typeIdentifier = reader.GetString();
        ComponentType = Type.GetType(typeIdentifier);
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(UUID);
        var typeName = ComponentType.FullName;
        data.Put(typeName);
        return data;
    }
}
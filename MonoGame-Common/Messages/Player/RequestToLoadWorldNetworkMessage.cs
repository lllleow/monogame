using LiteNetLib.Utils;
using MonoGame_Common.Attributes;

namespace MonoGame_Common.Messages.Player

{
    [NetworkMessage(3)]
    public class RequestToLoadWorldNetworkMessage : NetworkMessage
    {
        public RequestToLoadWorldNetworkMessage() : base()
        {
        }

        public override void Deserialize(NetDataReader reader)
        {
        }

        public override NetDataWriter Serialize()
        {
            var data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            return data;
        }
    }
}
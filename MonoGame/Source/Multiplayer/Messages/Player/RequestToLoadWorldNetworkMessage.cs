using LiteNetLib.Utils;

namespace MonoGame.Source.Multiplayer.Messages.Player

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
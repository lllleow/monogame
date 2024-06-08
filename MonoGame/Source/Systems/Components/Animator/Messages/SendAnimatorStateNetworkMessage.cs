using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer;

namespace MonoGame
{
    [NetworkMessage(12)]
    public class SendAnimatorStateNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public string CurrentState { get; set; }

        public SendAnimatorStateNetworkMessage()
        {
        }

        public SendAnimatorStateNetworkMessage(string uuid, string currentState)
        {
            UUID = uuid;
            CurrentState = currentState;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            CurrentState = reader.GetString();
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)GetNetworkTypeId());
            data.Put(UUID);
            data.Put(CurrentState);
            return data;
        }
    }
}
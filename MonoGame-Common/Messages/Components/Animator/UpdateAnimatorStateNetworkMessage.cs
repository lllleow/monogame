using LiteNetLib.Utils;
using MonoGame_Common.Attributes;

namespace MonoGame_Common.Messages.Components.Animator
{
    [NetworkMessage(13)]
    public class UpdateAnimatorStateNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public string TargetState { get; set; }

        public UpdateAnimatorStateNetworkMessage()
        {
        }

        public UpdateAnimatorStateNetworkMessage(string uuid, string targetState)
        {
            UUID = uuid;
            TargetState = targetState;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            TargetState = reader.GetString();
        }

        public override NetDataWriter Serialize()
        {
            var data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put(TargetState);
            return data;
        }
    }
}
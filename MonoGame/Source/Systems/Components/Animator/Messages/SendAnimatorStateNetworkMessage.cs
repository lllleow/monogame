using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Systems.Components.Animator;

namespace MonoGame
{
    [NetworkMessage(12)]
    public class SendAnimatorStateNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public string CurrentState { get; set; }
        public int CurrentTime { get; set; }
        public string AnimationBundleId { get; set; }

        public SendAnimatorStateNetworkMessage()
        {
        }

        public SendAnimatorStateNetworkMessage(string uuid, AnimatorComponent animatorComponent)
        {
            UUID = uuid;
            CurrentState = animatorComponent.GetCurrentStateId();
            CurrentTime = animatorComponent.StateMachine.CurrentState.CurrentTime;
            AnimationBundleId = animatorComponent.AnimationBundle.Id;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            CurrentState = reader.GetString();
            CurrentTime = reader.GetInt();
            AnimationBundleId = reader.GetString();
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put(CurrentState);
            data.Put(CurrentTime);
            data.Put(AnimationBundleId);
            return data;
        }
    }
}
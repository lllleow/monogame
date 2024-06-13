using LiteNetLib.Utils;
using MonoGame_Common.Attributes;

namespace MonoGame_Common.Messages.Components.Animator;

[NetworkMessage(12)]
public class SendAnimatorStateNetworkMessage : NetworkMessage
{
    public SendAnimatorStateNetworkMessage()
    {
    }

    public SendAnimatorStateNetworkMessage(string uuid, string currentComponentState, int currentTime, string animationBundleId)
    {
        UUID = uuid;
        CurrentState = currentComponentState;
        CurrentTime = currentTime;
        AnimationBundleId = animationBundleId;
    }

    public string UUID { get; set; } = "";
    public string CurrentState { get; set; } = "";
    public int CurrentTime { get; set; }
    public string AnimationBundleId { get; set; } = "";

    public override void Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        CurrentState = reader.GetString();
        CurrentTime = reader.GetInt();
        AnimationBundleId = reader.GetString();
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(UUID);
        data.Put(CurrentState);
        data.Put(CurrentTime);
        data.Put(AnimationBundleId);
        return data;
    }
}
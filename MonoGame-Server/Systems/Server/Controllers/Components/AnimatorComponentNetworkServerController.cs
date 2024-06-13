using MonoGame_Common.Messages.Components.Animator;
using MonoGame_Common.States.Components;

namespace MonoGame_Server.Systems.Server.Controllers.Components;

public class AnimatorComponentNetworkServerController : IServerNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<SendAnimatorStateNetworkMessage>((server, peer, message) =>
        {
            UpdateAnimatorStateNetworkMessage updateMessage = new()
            {
                UUID = message.UUID,
                TargetState = message.CurrentState
            };
            var state = NetworkServer.Instance.ServerWorld.GetEntityByUUID(message.UUID);
            var animatorState = new AnimatorComponentState
            {
                CurrentState = message.CurrentState,
                CurrentTime = message.CurrentTime,
                AnimationBundleId = message.AnimationBundleId
            };
            _ = state?.ReplaceComponent(animatorState);
            NetworkServer.Instance.BroadcastMessage(updateMessage, [peer]);
        });
    }

    public void Update()
    {
    }
}
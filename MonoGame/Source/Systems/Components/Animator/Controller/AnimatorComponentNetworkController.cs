using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame_Common.Messages.Components.Animator;

namespace MonoGame.Source.Systems.Components.Animator.Controller;

public class AnimatorComponentNetworkController : INetworkObjectController<AnimatorComponent>
{
    public void InitializeListeners(AnimatorComponent networkObject)
    {
        ClientNetworkEventManager.Subscribe<UpdateAnimatorStateNetworkMessage>(message =>
        {
            if (message.UUID == networkObject.Entity.UUID)
            {
                networkObject.SetState(message.TargetState);
            }
        });
    }

    public void SendStateUpdate(AnimatorComponent networkObject)
    {
        var message = new SendAnimatorStateNetworkMessage(
            networkObject.Entity.UUID,
            networkObject.StateMachine.CurrentState.Animation.Id,
            networkObject.StateMachine.CurrentState.CurrentTime,
            networkObject.StateMachine.AnimationBundle.Id);
        NetworkClient.SendMessage(message);
    }
}
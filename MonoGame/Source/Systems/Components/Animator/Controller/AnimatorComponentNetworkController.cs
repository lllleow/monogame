using System;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Systems.Components.Animator;

namespace MonoGame;

public class AnimatorComponentNetworkController : INetworkObjectController<AnimatorComponent>
{
    public void InitializeListeners(AnimatorComponent networkObject)
    {
        // ClientNetworkEventManager.Subscribe<UpdateAnimatorStateNetworkMessage>(message =>
        // {
        //     if (message.UUID == networkObject.Entity.UUID)
        //     {
        //         networkObject.SetState(message.TargetState);
        //     }
        // });
    }

    public void SendStateUpdate(AnimatorComponent networkObject)
    {
        var message = new SendAnimatorStateNetworkMessage(networkObject.Entity.UUID, networkObject.GetCurrentStateId());
        NetworkClient.SendMessage(message);
    }
}

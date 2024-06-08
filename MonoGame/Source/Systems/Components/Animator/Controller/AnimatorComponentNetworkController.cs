using System;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Systems.Components.Animator;

namespace MonoGame;

public class AnimatorComponentNetworkController : INetworkObjectController<AnimatorComponent>
{
    public void InitializeListeners(AnimatorComponent networkObject)
    {
        ClientNetworkEventManager.Subscribe<UpdateAnimatorStateNetworkMessage>(message =>
        {
            if (message.UUID == networkObject.Entity.UUID)
            {
                // networkObject.CurrentTime = message.CurrentTime;
                // networkObject.CurrentTextureX = message.TextureX;
                // networkObject.CurrentTextureY = message.TextureY;
                // TODO: Implement this
            }
        });
    }

    public void SendStateUpdate(AnimatorComponent networkObject)
    {
        // var message = new SendAnimatorStateNetworkMessage(networkObject.Entity.UUID, networkObject.CurrentTime, networkObject.CurrentTextureX, networkObject.CurrentTextureY);
        // NetworkClient.SendMessage(message);
    }
}

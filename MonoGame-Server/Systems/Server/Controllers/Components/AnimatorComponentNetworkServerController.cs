using MonoGame;
using MonoGame_Server.Systems.Server;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.States;
using MonoGame.Source.States.Components;

namespace MonoGame_Server;

public class AnimatorComponentNetworkServerController : IStandaloneNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<SendAnimatorStateNetworkMessage>((server, peer, message) =>
        {
            UpdateAnimatorStateNetworkMessage updateMessage = new(message.UUID, message.CurrentTime, message.CurrentTextureX, message.CurrentTextureY);
            EntityState? state = NetworkServer.Instance.ServerWorld.GetEntityByUUID(message.UUID);
            AnimatorComponentState animatorState = new AnimatorComponentState
            {
                CurrentTime = message.CurrentTime,
                TextureX = message.CurrentTextureX,
                TextureY = message.CurrentTextureY
            };
            state?.SetComponent(animatorState);
            NetworkServer.Instance.BroadcastMessage(updateMessage, blacklist: [peer]);
        });
    }
}

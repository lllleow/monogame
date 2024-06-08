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
            UpdateAnimatorStateNetworkMessage updateMessage = new(message.UUID, message.CurrentState);
            EntityState? state = NetworkServer.Instance.ServerWorld.GetEntityByUUID(message.UUID);
            AnimatorComponentState animatorState = new AnimatorComponentState
            {
                CurrentState = message.CurrentState,
            };
            state?.ReplaceComponent(animatorState);
            NetworkServer.Instance.BroadcastMessage(updateMessage, blacklist: [peer]);
        });
    }
}

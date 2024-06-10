using MonoGame_Common.Messages.Components;
using MonoGame_Common.States.Components;

namespace MonoGame_Server.Systems.Server.Controllers;

public class ComponentNetworkServerController : IServerNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<RegisterEntityComponentNetworkMessage>((server, peer, message) =>
        {
            var entityState = server.ServerWorld.GetEntityByUUID(message.UUID);
            if (entityState != null && !entityState.HasComponent(message.ComponentType))
            {
                var newComponentState = Activator.CreateInstance(message.ComponentType) as ComponentState;
                _ = entityState.AddComponent(newComponentState);
            }
        });
    }
}

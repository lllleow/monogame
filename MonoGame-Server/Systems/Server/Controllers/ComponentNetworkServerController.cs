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
            Type? componentType = message.ComponentType;
            if (entityState != null && componentType != null && !entityState.HasComponent(componentType))
            {
                var newComponentState = Activator.CreateInstance(componentType) as ComponentState;
                if (newComponentState == null) return;
                _ = entityState.AddComponent(newComponentState);
            }
        });
    }

    public void Update()
    {
    }
}
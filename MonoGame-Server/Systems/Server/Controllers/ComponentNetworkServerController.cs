using System.ComponentModel;
using Microsoft.Xna.Framework;
using MonoGame;
using MonoGame_Server.Systems.Saving;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.States;
using MonoGame.Source.States.Components;
using MonoGame.Source.Systems.Components.Interfaces;

namespace MonoGame_Server;

public class ComponentNetworkServerController : IStandaloneNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<RegisterEntityComponentNetworkMessage>((server, peer, message) =>
        {
            EntityState? entityState = server.ServerWorld.GetEntityByUUID(message.UUID);
            if (entityState != null && !entityState.HasComponent(message.ComponentType))
            {
                ComponentState? newComponentState = Activator.CreateInstance(message.ComponentType) as ComponentState;
                entityState.AddComponent(newComponentState);
            }
        });
    }
}

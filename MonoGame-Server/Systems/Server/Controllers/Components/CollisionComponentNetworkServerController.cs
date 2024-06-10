using MonoGame_Common.Messages.Components.Collision;
using MonoGame_Common.States.Components;

namespace MonoGame_Server.Systems.Server.Controllers.Components;

public class CollisionComponentNetworkServerController : IServerNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<SendCollisionModeUpdateNetworkMessage>((server, peer, message) =>
        {
            var entity = NetworkServer.Instance.ServerWorld.GetEntityByUUID(message.UUID);
            if (entity != null && (entity?.HasComponent(typeof(CollisionComponentState)) ?? false))
            {
                entity.GetComponent<CollisionComponentState>().Mode = message.Mode;

                var setCollisionModeNetworkMessage = new SetCollisionModeNetworkMessage(message.UUID, message.Mode);
                NetworkServer.Instance.BroadcastMessage(setCollisionModeNetworkMessage);
            }
        });
    }
}

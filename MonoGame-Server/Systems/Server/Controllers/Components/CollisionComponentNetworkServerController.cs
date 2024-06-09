using MonoGame;
using MonoGame_Server.Systems.Server;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.States;

namespace MonoGame_Server;

public class CollisionComponentNetworkServerController : IStandaloneNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<SendCollisionModeUpdateNetworkMessage>((server, peer, message) =>
        {
            EntityState? entity = NetworkServer.Instance.ServerWorld.GetEntityByUUID(message.UUID);
            if (entity != null && (entity?.HasComponent(typeof(CollisionComponentState)) ?? false))
            {
                entity.GetComponent<CollisionComponentState>().Mode = message.Mode;

                SetCollisionModeNetworkMessage setCollisionModeNetworkMessage = new SetCollisionModeNetworkMessage(message.UUID, message.Mode);
                NetworkServer.Instance.BroadcastMessage(setCollisionModeNetworkMessage);
            }
        });
    }
}

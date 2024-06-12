using MonoGame_Common.Messages.Components.Collision;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame.Source.Systems.Components.Collision.Controller;

public class CollisionComponentNetworkController : INetworkObjectController<CollisionComponent>
{
    public void InitializeListeners(CollisionComponent networkObject)
    {
        ClientNetworkEventManager.Subscribe<SetCollisionModeNetworkMessage>(message =>
        {
            if (networkObject.Entity.UUID == message.UUID)
            {
                networkObject.Mode = message.Mode;
            }
        });
    }

    public void SetCollisionMode(CollisionComponent networkObject)
    {
        var message = new SendCollisionModeUpdateNetworkMessage
        {
            UUID = networkObject.Entity.UUID,
            Mode = networkObject.Mode
        };
        NetworkClient.SendMessage(message);
    }
}
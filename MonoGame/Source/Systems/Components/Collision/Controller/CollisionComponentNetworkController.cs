using System;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Systems.Components.Collision;

namespace MonoGame;

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
        SendCollisionModeUpdateNetworkMessage message = new SendCollisionModeUpdateNetworkMessage();
        message.UUID = networkObject.Entity.UUID;
        message.Mode = networkObject.Mode;
        NetworkClient.SendMessage(message);
    }
}

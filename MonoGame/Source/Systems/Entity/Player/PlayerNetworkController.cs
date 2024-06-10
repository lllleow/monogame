using MonoGame_Common.Messages.Player;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame.Source.Systems.Entity.Player;

public class PlayerNetworkController : INetworkObjectController<Player>
{
    public void InitializeListeners(Player player)
    {
        ClientNetworkEventManager.Subscribe<UpdatePlayerPositionNetworkMessage>(message =>
        {
            if (player.UUID == message.UUID) player.Position = message.Position;
        });
    }
}
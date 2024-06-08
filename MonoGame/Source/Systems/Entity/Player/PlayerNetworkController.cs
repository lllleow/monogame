using System;
using Microsoft.Xna.Framework;
using MonoGame.Source;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.Messages.Player;
using MonoGame.Source.Systems.Entity.PlayerNamespace;

namespace MonoGame;

public class PlayerNetworkController : INetworkObjectController<Player>
{
    public void InitializeListeners(Player player)
    {
        ClientNetworkEventManager.Subscribe<MovePlayerNetworkMessage>(message =>
        {
            if (player.UUID == message.UUID)
            {
                if (Vector2.Distance(player.Position, message.ExpectedPosition) < 1f)
                {
                    player.Position = message.ExpectedPosition;
                }
                else
                {
                    player.Move(Globals.GameTime, message.Direction, message.Speed);
                }
            }
        });

        ClientNetworkEventManager.Subscribe<UpdatePlayerPositionNetworkMessage>(message =>
        {
            if (player.UUID == message.UUID)
            {
                player.Position = message.Position;
            }
        });
    }
}

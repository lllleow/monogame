using LiteNetLib;
using Microsoft.Xna.Framework;
using MonoGame.Source;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessageHandler.Client;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;
using MonoGame.Source.Util.Enum;

namespace MonoGame_Server.Systems.Server.MessageHandlers;

public class RequestMovementNetworkMessageHandler : IServerMessageHandler
{
    private readonly NetworkServer networkServer = NetworkServer.Instance;

    public void Execute(NetPeer peer, byte channel, INetworkMessage message)
    {
        var requestMovementNetworkMessage = (RequestMovementNetworkMessage)message;
        var playerState = NetworkServer.Instance.GetPlayerFromPeer(peer);

        var displacement = GetDisplacement(requestMovementNetworkMessage.Direction, requestMovementNetworkMessage.Speed);
        var newPosition = (playerState.Position ?? Globals.SpawnPosition) + displacement;
        playerState.Position = newPosition;

        networkServer.BroadcastMessage(new MovePlayerNetworkMessage(playerState.UUID, requestMovementNetworkMessage.Speed, requestMovementNetworkMessage.Direction, newPosition));
    }

    public bool Validate(NetPeer peer, byte channel, INetworkMessage message)
    {
        var requestMovementNetworkMessage = (RequestMovementNetworkMessage)message;
        return requestMovementNetworkMessage.Speed.LengthSquared() <= Vector2.One.LengthSquared();
    }

    public Vector2 GetDisplacement(Direction direction, Vector2 speed)
    {
        return direction switch
        {
            Direction.Up => new Vector2(0, -speed.Y),
            Direction.Down => new Vector2(0, speed.Y),
            Direction.Left => new Vector2(-speed.X, 0),
            Direction.Right => new Vector2(speed.X, 0),
            Direction.LeftUp => new Vector2(-speed.X, -speed.Y),
            Direction.RightUp => new Vector2(speed.X, -speed.Y),
            Direction.LeftDown => new Vector2(-speed.X, speed.Y),
            Direction.RightDown => new Vector2(speed.X, speed.Y),
            _ => Vector2.Zero,
        };
    }
}

using LiteNetLib;
using Microsoft.Xna.Framework;
using MonoGame;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame_Server.Systems.Server;
using MonoGame_Server.Systems.Server.MessageHandlers;

namespace MonoGame_Server;

public class RequestMovementNetworkMessageHandler : IServerMessageHandler
{
    NetworkServer NetworkServer = NetworkServer.Instance;

    public void Execute(NetPeer peer, byte channel, INetworkMessage message)
    {
        RequestMovementNetworkMessage requestMovementNetworkMessage = (RequestMovementNetworkMessage)message;
        PlayerState playerState = NetworkServer.Instance.GetPlayerFromPeer(peer);

        Vector2 displacement = GetDisplacement(requestMovementNetworkMessage.Direction, requestMovementNetworkMessage.Speed);
        Vector2 newPosition = (playerState.Position ?? Globals.spawnPosition) + displacement;
        playerState.Position = newPosition;

        NetworkServer.BroadcastMessage(new MovePlayerNetworkMessage(playerState.UUID, requestMovementNetworkMessage.Speed, requestMovementNetworkMessage.Direction, newPosition));
    }

    public bool Validate(NetPeer peer, byte channel, INetworkMessage message)
    {
        RequestMovementNetworkMessage requestMovementNetworkMessage = (RequestMovementNetworkMessage)message;
        if (requestMovementNetworkMessage.Speed.LengthSquared() <= Vector2.One.LengthSquared())
        {
            return true;
        }

        return false;
    }

    public Vector2 GetDisplacement(Direction direction, Vector2 speed)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector2(0, -speed.Y);
            case Direction.Down:
                return new Vector2(0, speed.Y);
            case Direction.Left:
                return new Vector2(-speed.X, 0);
            case Direction.Right:
                return new Vector2(speed.X, 0);
            case Direction.LeftUp:
                return new Vector2(-speed.X, -speed.Y);
            case Direction.RightUp:
                return new Vector2(speed.X, -speed.Y);
            case Direction.LeftDown:
                return new Vector2(-speed.X, speed.Y);
            case Direction.RightDown:
                return new Vector2(speed.X, speed.Y);
            default:
                return Vector2.Zero;
        }
    }
}

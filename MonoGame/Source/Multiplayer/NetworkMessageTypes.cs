using System;
using MonoGame.Source.Multiplayer.NetworkMessageHandler.Client;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessageHandler.Client;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;

namespace MonoGame.Source.Multiplayer;

public enum NetworkMessageTypes
{
    AuthenticateUserNetworkMessage = 0,
    AuthenticationResultNetworkMessage = 1,
    RequestToLoadWorldNetworkMessage = 2,
    ChunkDataNetworkMessage = 3,
    SpawnPlayerNetworkMessage = 4,
    RequestMovementNetworkMessage = 5,
    UpdatePlayerPositionNetworkMessage = 6,
    MovePlayerNetworkMessage = 7,
    RequestToPlaceTileNetworkMessage = 8,
    PlaceTileNetworkMessage = 9,
    DeleteTileNetworkMessage = 10
}

public static class NetworkMessageTypeClientHelper
{
    public static Type GetTypeFromMessageType(NetworkMessageTypes messageType)
    {
        return messageType switch
        {
            NetworkMessageTypes.AuthenticateUserNetworkMessage => typeof(AuthenticateUserNetworkMessage),
            NetworkMessageTypes.AuthenticationResultNetworkMessage => typeof(AuthenticationResultNetworkMessage),
            NetworkMessageTypes.RequestToLoadWorldNetworkMessage => typeof(RequestToLoadWorldNetworkMessage),
            NetworkMessageTypes.ChunkDataNetworkMessage => typeof(ChunkDataNetworkMessage),
            NetworkMessageTypes.SpawnPlayerNetworkMessage => typeof(SpawnPlayerNetworkMessage),
            NetworkMessageTypes.RequestMovementNetworkMessage => typeof(RequestMovementNetworkMessage),
            NetworkMessageTypes.UpdatePlayerPositionNetworkMessage => typeof(UpdatePlayerPositionNetworkMessage),
            NetworkMessageTypes.MovePlayerNetworkMessage => typeof(MovePlayerNetworkMessage),
            NetworkMessageTypes.RequestToPlaceTileNetworkMessage => typeof(RequestToPlaceTileNetworkMessage),
            NetworkMessageTypes.PlaceTileNetworkMessage => typeof(PlaceTileNetworkMessage),
            NetworkMessageTypes.DeleteTileNetworkMessage => typeof(DeleteTileNetworkMessage),
            _ => null,
        };
    }

    public static Type GetHandlerForClientMessageType(NetworkMessageTypes messageTypes)
    {
        return messageTypes switch
        {
            NetworkMessageTypes.AuthenticationResultNetworkMessage => typeof(AuthenticationResultNetworkMessageHandler),
            NetworkMessageTypes.ChunkDataNetworkMessage => typeof(ChunkDataNetworkMessageHandler),
            NetworkMessageTypes.SpawnPlayerNetworkMessage => typeof(SpawnPlayerNetworkMessageHandler),
            NetworkMessageTypes.UpdatePlayerPositionNetworkMessage => typeof(UpdatePlayerPositionNetworkMessageHandler),
            NetworkMessageTypes.MovePlayerNetworkMessage => typeof(MovePlayerNetworkMessageHandler),
            NetworkMessageTypes.PlaceTileNetworkMessage => typeof(PlaceTileNetworkMessageHandler),
            NetworkMessageTypes.DeleteTileNetworkMessage => typeof(DeleteTileNetworkMessageHandler),
            _ => null,
        };
    }
}

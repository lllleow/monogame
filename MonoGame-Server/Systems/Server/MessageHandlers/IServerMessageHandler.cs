using LiteNetLib;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame_Server.Systems.Server.MessageHandlers;

public interface IServerMessageHandler
{
    public abstract bool Validate(NetPeer peer, byte channel, INetworkMessage message);
    public abstract void Execute(NetPeer peer, byte channel, INetworkMessage message);
}

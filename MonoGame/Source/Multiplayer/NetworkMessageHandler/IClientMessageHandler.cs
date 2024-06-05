using System;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame.Source.Multiplayer.NetworkMessageHandler;

public interface IClientMessageHandler
{
    public abstract void Execute(byte channel, INetworkMessage message);
}

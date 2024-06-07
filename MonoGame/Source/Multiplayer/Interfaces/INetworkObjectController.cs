using System;
namespace MonoGame.Source.Multiplayer.Interfaces;

public interface INetworkObjectController<T> : INetworkController
{
    public abstract void InitializeListeners(T networkObject);
}

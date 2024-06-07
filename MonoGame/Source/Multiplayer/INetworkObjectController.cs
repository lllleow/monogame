using System;
namespace MonoGame;

public interface INetworkObjectController<T> : INetworkController
{
    public abstract void InitializeListeners(T networkObject);
}

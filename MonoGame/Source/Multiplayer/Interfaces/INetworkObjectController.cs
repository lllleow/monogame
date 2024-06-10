namespace MonoGame.Source.Multiplayer.Interfaces;

public interface INetworkObjectController<T> : INetworkController
{
    public void InitializeListeners(T networkObject);
}
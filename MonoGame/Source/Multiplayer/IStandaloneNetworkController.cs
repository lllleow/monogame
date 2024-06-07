using System;
namespace MonoGame;

public interface IStandaloneNetworkController : INetworkController
{
    public abstract void InitializeListeners();
}

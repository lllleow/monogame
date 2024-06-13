namespace MonoGame_Server.Systems.Server;

public interface IServerNetworkController
{
    public void InitializeListeners();
    public void Update();
}
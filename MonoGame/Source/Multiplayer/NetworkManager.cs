using System;
using LiteNetLib;

namespace MonoGame;

public class NetworkManager
{
    private NetworkClient client;
    private NetworkServer server;

    public NetworkManager()
    {
        switch (Globals.networkMode)
        {
            case NetworkMode.Server:
                server = new NetworkServer();
                break;
            case NetworkMode.Client:
                client = new NetworkClient();
                break;
        }
    }

    public void Initialize()
    {
        switch (Globals.networkMode)
        {
            case NetworkMode.Server:
                server?.InitializeServer();
                break;
            case NetworkMode.Client:
                client?.InitializeClient();
                break;
        }
    }

    public void Update()
    {
        switch (Globals.networkMode)
        {
            case NetworkMode.Server:
                server?.Update();
                break;
            case NetworkMode.Client:
                client?.Update();
                break;
        }
    }

    public NetworkClient GetClient()
    {
        return client;
    }

    public NetworkServer GetServer()
    {
        return server;
    }
}

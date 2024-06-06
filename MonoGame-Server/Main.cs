using MonoGame_Server.Systems.Server;

namespace MonoGame_Server;

internal class Server
{
    public static void Main(string[] args)
    {
        NetworkServer server = NetworkServer.Instance;
        server.InitializeServer();

        while (true)
        {
            server.Update();
        }
    }
}
using MonoGame;
using MonoGame_Server.Systems.Server;

namespace MonoGame_Server;

class Server
{
    public static void Main(string[] args)
    {
        var server = NetworkServer.Instance;
        server.InitializeServer();

        while (true)
        {
            server.Update();
        }
    }
}
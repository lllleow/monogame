using System.Timers;
using MonoGame_Server.Systems.Server;

namespace MonoGame_Server;

internal class Server
{
    private static System.Timers.Timer? timer;
    private static NetworkServer? server;

    public static void Main(string[] args)
    {
        server = NetworkServer.Instance;
        server.InitializeServer();
        SetupFixedTimer();
        Console.WriteLine("Server is running. Press Enter to exit...");
        Console.ReadLine();
        timer?.Stop();
        timer?.Dispose();
        Console.WriteLine("Server stopped.");
    }

    private static void SetupFixedTimer()
    {
        timer = new System.Timers.Timer(0.0001);
        timer.Elapsed += Update;
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    private static void Update(object? source, ElapsedEventArgs e)
    {
        server?.Update();
    }
}
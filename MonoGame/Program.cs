using System;
using System.IO;
using MonoGame.Source;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Globals.Args = args;
        using var game = new MonoGame.Main();
        game.Run();
    }
}

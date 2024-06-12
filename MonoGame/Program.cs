using System;
using MonoGame;
using MonoGame.Source;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Globals.Args = args;
        using var game = new Main();
        game.Run();
    }
}
using MonoGame;
using MonoGame.Source;
using System;

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
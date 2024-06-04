using System;
using MonoGame;

public static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Globals.args = args;
        using (var game = new MonoGame.Main())
        {
            game.Run();
        }
    }
}

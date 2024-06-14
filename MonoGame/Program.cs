using System;
using System.IO;
using MonoGame;
using MonoGame.Source;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Globals.Args = args;
        try
        {
            using var game = new Main();
            game.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            using (var writer = new StreamWriter("error.log", true))
            {
                writer.WriteLine(e.ToString());
            }
        }
    }
}
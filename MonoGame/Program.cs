using System;
using System.IO;
using MonoGame.Source;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            Globals.Args = args;
            using var game = new MonoGame.Main();
            game.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            File.WriteAllText("error.log", ex.ToString());
        }
    }
}

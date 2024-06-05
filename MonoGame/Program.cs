using System;
using System.IO;
using MonoGame;

public static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            Globals.args = args;
            using (var game = new MonoGame.Main())
            {
                game.Run();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            File.WriteAllText("error.log", ex.ToString());
        }
    }
}

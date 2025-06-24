using System;
using System.Reflection;
using NLog;

namespace SnakeGame.DesktopGL;

public static class Program
{
    private readonly static Logger Logger = LogManager.GetCurrentClassLogger();
    
    public static void Main(string[] args)
    {
        var game = new SnakeGame();
        try
        {
            Logger.Info("Starting game...");
            
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Logger.Info($"Version: {version}");
            
            game.Run();
        }
        catch (Exception ex)
        {
            Logger.Fatal("Unhandled exception");
            Logger.Fatal(ex);
        }
        finally
        {
            game.Dispose();
        }
    }
}
using System.Reflection;

namespace SnakeGame.Core.Utils;

public class VersionUtils
{
    public static string GetVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        
        if (version == null)
            return string.Empty;
        
        return $"{version.Major}.{version.Minor}";
    }
}
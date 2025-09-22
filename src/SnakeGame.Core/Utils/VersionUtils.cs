using System.Reflection;
using System.Text;

namespace SnakeGame.Core.Utils;

public static class VersionUtils
{
    public static string GetVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        
        if (version == null)
            return string.Empty;
        
        var sb = new StringBuilder($"{version.Major}.{version.Minor}");
        
        if (version.Build > 0)
            sb.Append($".{version.Build}");
        
        if (version.Revision > 0)
            sb.Append($".{version.Revision}");
        
        return sb.ToString();
    }
}
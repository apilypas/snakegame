namespace SnakeGame.DesktopGL;

public static class Constants
{
    public const int SegmentSize = 20; // Pixels
    public const int WallWidth = 32; // Segments
    public const int WallHeight = 32; // Segments
    public const float BugSpawnRate = 3f; // Seconds
    public const float SpeedBugSpawnRate = 7f; // Seconds
    public const int MaxBugLimit = 5;
    public const int MaxSpeedBugLimit = 2;
    public const int BugKillScore = 1; // Points
    public const int SnakePartKillScore = 1; // Points
    public const int SpeedBugKillScore = 2; // Points
    public const int InitialSnakeSize = 3; // Segments
    public const float DefaultSnakeSpeed = 100f;
    public const float IncreasedSnakeSpeed = 200f;
    public const float SpeedUpRate = 10f; // Seconds
}

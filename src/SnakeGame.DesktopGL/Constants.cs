namespace SnakeGame.DesktopGL;

public static class Constants
{
    public const int SegmentSize = 20; // Pixels
    public const int WallWidth = 32; // Segments
    public const int WallHeight = 32; // Segments
    public const float DiamondSpawnRate = 3f; // Seconds
    public const float SpeedBoostSpawnRate = 7f; // Seconds
    public const float EnemySpawnRate = 5f;
    public const int MaxEnemies = 8;
    public const int MaxDiamondLimit = 5;
    public const int MaxSpeedBoostLimit = 2;
    public const int DiamondCollectScore = 1; // Points
    public const int SnakePartCollectScore = 1; // Points
    public const int SpeedBoostCollectScore = 2; // Points
    public const float DefaultSnakeSpeed = 100f;
    public const float IncreasedSnakeSpeed = 200f;
    public const float SpeedUpRate = 10f; // Seconds
}

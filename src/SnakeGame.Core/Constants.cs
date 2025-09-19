namespace SnakeGame.Core;

public static class Constants
{
    public const int SegmentSize = 16; // Pixels
    public const int WallWidth = 32; // Segments
    public const int WallHeight = 20; // Segments
    public const float DiamondSpawnRate = 3f; // Seconds
    public const float SpeedBoostSpawnRate = 7f; // Seconds
    public const float CrownSpawnRate = 30f; // Seconds
    public const float EnemySpawnRate = 5f;
    public const int MaxEnemies = 8;
    public const int MaxDiamondLimit = 5;
    public const int MaxSpeedBoostLimit = 2;
    public const int MaxCrownLimit = 1;
    public const int DiamondCollectScore = 1; // Points
    public const int SnakePartCollectScore = 1; // Points
    public const int SpeedBoostCollectScore = 2; // Points
    public const int CrownCollectScore = 5; // Points
    public const int ClockCollectScore = 3; // Points
    public const float DefaultSnakeSpeed = 100f;
    public const float IncreasedSnakeSpeed = 200f;
    public const float SpeedUpTimer = 10f; // Seconds
    public const float InvincibleTimer = 8f; // Seconds
    public const float InitialTimer = 60f; // Seconds
    public const float MaxTimer = 300f; // Seconds
    public const int VirtualScreenWidth = 640;
    public const int VirtualScreenHeight = 360;
    public const int InitialSnakeSize = 3;
    public const float ClockBonus = 30f; // Seconds
    public const float ScoreMultiplicatorTimer = 10f; // Seconds
    public const int MaxScoreBoardEntries = 15;
    public const int MaxScoreMultiplier = 666;
    public const string ScoreFormat = "00000000";
}

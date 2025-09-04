using System.Collections.Generic;
using MonoGame.Extended.ECS;

namespace SnakeGame.Core.Data;

public class GameState
{
    public int Deaths { get; set; }
    public int ScoreMultiplicator { get; set; } = 1;
    public int Score { get; set; }
    public int LongestSnake { get; set; } = 3;
    public float Timer { get; set; } = Constants.InitialTimer;
    public int TimerRounded { get; set; } = (int)Constants.InitialTimer;
    public float ScoreMultiplicatorTimer { get; set; }
    public float TotalTime { get; set; }
    public bool IsPaused { get; set; }
    public GameWorldState State { get; set; } = GameWorldState.Running;
    
    public HashSet<Entity> Snakes { get; } = [];
    public HashSet<Entity> Collectables { get; } = [];
    public Entity PlayerSnake { get; set; }
    public int ScoreLabelId { get; set; }
    public int MultiplicatorLabelId { get; set; }
    public int TimeLabelId { get; set; }
    public int PausedDialogId { get; set; }
    public int GameOverDialogId { get; set; }
    public int ScoreBoardDialogId { get; set; }
}
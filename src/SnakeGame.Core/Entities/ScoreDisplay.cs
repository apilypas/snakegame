using System;
using System.Text;

namespace SnakeGame.Core.Entities;

public class ScoreDisplay : Entity
{
    private readonly Label _scoreLabel;
    
    public int Score { get; private set; }
    public int Deaths { get; private set; }
    public int LongestSnake { get; private set; } = Constants.InitialSnakeSize;
    public int Timer { get; private set; } = (int)Constants.InitialTimer;

    public ScoreDisplay()
    {
        _scoreLabel = new Label();

        AddChild(_scoreLabel);
        
        UpdateTexts();
    }
    
    public void UpdateDeaths(Snake snake)
    {
        if (snake is not PlayerSnake)
            return;

        Deaths++;
        UpdateTexts();
    }
    
    public void UpdateTimer(int timer)
    {
        if (Timer == timer)
            return;

        Timer = timer;
        UpdateTexts();
    }
    
    public void UpdateScore(Collectable collectable, Snake snake)
    {
        if (snake is not PlayerSnake playerSnake)
            return;
        
        switch (collectable.Type)
        {
            case CollectableType.Diamond:
                Score += Constants.DiamondCollectScore;
                break;
            case CollectableType.SnakePart:
                Score += Constants.SnakePartCollectScore;
                break;
            case CollectableType.SpeedBoost:
                Score += Constants.SpeedBoostCollectScore;
                break;
            case CollectableType.Clock:
                Score += Constants.ClockCollectScore;
                break;
        }

        LongestSnake = Math.Max(LongestSnake, playerSnake.Segments.Count);

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        _scoreLabel.Text = new StringBuilder()
            .AppendLine($"Score: {Score}")
            .AppendLine($"Timer: {Timer / 60:00}:{Timer % 60:00}")
            .AppendLine($"Deaths: {Deaths}")
            .ToString();
    }
}
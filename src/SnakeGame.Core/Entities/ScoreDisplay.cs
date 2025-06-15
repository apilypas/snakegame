using System.Text;
using SnakeGame.Core.Events;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class ScoreDisplay : Entity
{
    private readonly Label _scoreLabel;

    private int _score;
    private int _deaths;
    private int _longestSnake = Constants.InitialSnakeSize;
    private int _timer = (int)Constants.InitialTimer;
    private int _scoreMultiplicator = 1;

    public ScoreDisplay(EventBus eventBus)
    {
        _scoreLabel = new Label();

        eventBus.Subscribe<TimerChangedEvent>(OnTimerChanged);
        eventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);
        eventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
        eventBus.Subscribe<LongestSnakeChanged>(OnLongestSnakeChanged);
        eventBus.Subscribe<ScoreMultiplicatorChangedEvent>(OnScoreMultiplayerChanged);
        
        AddChild(_scoreLabel);
        
        UpdateTexts();
    }

    private void OnPlayerDied(PlayerDiedEvent e)
    {
        _deaths = e.TotalDeaths;
        UpdateTexts();
    }

    private void OnTimerChanged(TimerChangedEvent e)
    {
        _timer = e.Timer;
        
        UpdateTexts();
    }

    private void OnScoreChanged(ScoreChangedEvent e)
    {
        _score = e.Score;

        UpdateTexts();
    }

    private void OnLongestSnakeChanged(LongestSnakeChanged e)
    {
        _longestSnake = e.Length;
        
        UpdateTexts();
    }
    
    private void OnScoreMultiplayerChanged(ScoreMultiplicatorChangedEvent e)
    {
        _scoreMultiplicator = e.ScoreMultiplicator;

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        _scoreLabel.Text = new StringBuilder()
            .AppendLine($"Score: {_score}")
            .AppendLine($"Timer: {_timer / 60:00}:{_timer % 60:00}")
            .AppendLine($"Deaths: {_deaths}")
            .AppendLine($"Longest Snake: {_longestSnake}")
            .AppendLine($"Score multiplicator: {_scoreMultiplicator}")
            .ToString();
    }
}
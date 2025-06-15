using System.Text;

namespace SnakeGame.Core.Entities;

public class ScoreDisplay : Entity
{
    private readonly Label _scoreLabel;

    private int _score;
    private int _deaths;
    private int _longestSnake = Constants.InitialSnakeSize;
    private int _timer = (int)Constants.InitialTimer;
    private int _scoreMultiplicator = 1;

    public ScoreDisplay()
    {
        _scoreLabel = new Label();

        AddChild(_scoreLabel);
        
        UpdateTexts();
    }
    
    public void SetDeaths(int deaths)
    {
        _deaths = deaths;
        UpdateTexts();
    }
    
    public void SetTimer(int timer)
    {
        if (_timer == timer)
            return;

        _timer = timer;
        
        UpdateTexts();
    }
    
    public void SetScore(int score)
    {
        _score = score;

        UpdateTexts();
    }

    public void SetLongestSnake(int longest)
    {
        _longestSnake = longest;
        
        UpdateTexts();
    }

    public void SetScoreMultiplicator(int scoreMultiplicator)
    {
        _scoreMultiplicator = scoreMultiplicator;
        
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
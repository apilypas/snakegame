using System;
using System.Text;
using SnakeGame.Core.Events;
using SnakeGame.Core.Systems;

namespace SnakeGame.Core.Entities;

public class ScoreDisplay : Entity, IObserver
{
    private readonly Label _scoreLabel;
    
    private float _timer = Constants.InitialTimer;
    
    public int Score { get; private set; }
    public int Deaths { get; private set; }
    public int LongestSnake { get; private set; } = Constants.InitialSnakeSize;

    public ScoreDisplay(AssetManager assets)
    {
        _scoreLabel = new Label
        {
            Font = assets.MainFont,
            Color = Colors.DefaultTextColor
        };

        Children.Add(_scoreLabel);
        
        UpdateTexts();
    }

    public void Notify(NotifyEvent notifyEvent)
    {
        if (notifyEvent.EventType == NotifyEventType.CollectableRemoved)
            OnCollectableRemoved(notifyEvent);

        if (notifyEvent.EventType == NotifyEventType.TimerChanged)
            OnTimerChanged((NotifyTimerChangedEvent)notifyEvent);

        if (notifyEvent.EventType == NotifyEventType.SnakeDied)
            OnDeathsChanged(notifyEvent);
    }

    private void OnDeathsChanged(NotifyEvent notifyEvent)
    {
        if (notifyEvent.Target is not PlayerSnake)
            return;

        Deaths++;
        UpdateTexts();
    }

    private void OnTimerChanged(NotifyTimerChangedEvent notifyEvent)
    {
        _timer = notifyEvent.Timer;
        UpdateTexts();
    }

    private void OnCollectableRemoved(NotifyEvent notifyEvent)
    {
        if (notifyEvent.Target is not Collectable collectable)
            return;

        if (notifyEvent.TriggeredBy is not PlayerSnake playerSnake)
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
            .AppendLine($"Timer: {(int)(_timer / 60):00}:{(int)(_timer % 60):00}")
            .AppendLine($"Deaths: {Deaths}")
            .ToString();
    }
}
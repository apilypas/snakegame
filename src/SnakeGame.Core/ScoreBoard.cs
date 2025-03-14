using System.Text;
using SnakeGame.Core.Entities;
using SnakeGame.Core.Events;

namespace SnakeGame.Core;

public class ScoreBoard : IObserver
{
    private float _timer = Constants.InitialTimer;

    public string DisplayText { get; private set; } = string.Empty;
    public int Score { get; private set; } = 0;
    public int Deaths { get; private set; } = 0;

    public ScoreBoard()
    {
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

        if (notifyEvent.TriggeredBy is not PlayerSnake)
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
        }

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        DisplayText = new StringBuilder()
            .AppendLine($"Score: {Score}")
            .AppendLine($"Timer: {(int)(_timer / 60):00}:{(int)(_timer % 60):00}")
            .AppendLine($"Deaths: {Deaths}")
            .ToString();
    }
}
using SnakeGame.DesktopGL;
using SnakeGame.DesktopGL.Core.Entities;
using SnakeGame.DesktopGL.Core.Events;

public class ScoreBoard : IObserver
{
    private int _score = 0;

    public string ScoreText { get; private set; }

    public ScoreBoard()
    {
        UpdateTexts();
    }

    public void Notify(NotifyEvent notifyEvent)
    {
        if (notifyEvent.EventType == NotifyEventType.CollectableRemoved)
            OnCollectableRemoved(notifyEvent);
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
                _score += Constants.DiamondCollectScore;
                break;
            case CollectableType.SnakePart:
                _score += Constants.SnakePartCollectScore;
                break;
            case CollectableType.SpeedBoost:
                _score += Constants.SpeedBoostCollectScore;
                break;
        }

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        ScoreText = $"Score: {_score}";
    }
}
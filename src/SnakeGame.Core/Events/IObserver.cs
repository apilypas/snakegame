namespace SnakeGame.Core.Events;

public interface IObserver
{
    void Notify(NotifyEvent notifyEvent);
}

namespace SnakeGame.DesktopGL.Core.Events;

public interface IObserver
{
    void Notify(NotifyEvent notifyEvent);
}

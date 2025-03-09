using System.Collections.Generic;

namespace SnakeGame.Core.Events;

public class EventManager
{
    private readonly IList<IObserver> _observers = [];

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(NotifyEvent notifyEvent)
    {
        foreach (var observer in _observers)
        {
            observer.Notify(notifyEvent);
        }
    }
}

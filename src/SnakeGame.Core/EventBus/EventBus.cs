using System;
using System.Collections.Generic;

namespace SnakeGame.Core.EventBus;

public class EventBus
{
    private readonly Dictionary<Type, Delegate> _events = new();

    public void Subscribe<T>(Action<T> callback) where T : struct
    {
        if (_events.TryGetValue(typeof(T), out var handler))
        {
            _events[typeof(T)] = Delegate.Combine(handler, callback);
        }
        else
        {
            _events[typeof(T)] = callback;
        }
    }

    public void Unsubscribe<T>(Action<T> callback) where T : struct
    {
        if (_events.TryGetValue(typeof(T), out var handler))
        {
            _events[typeof(T)] = Delegate.Remove(handler, callback);
        }
    }

    public void Publish<T>(T message)  where T : struct
    {
        if (_events.TryGetValue(typeof(T), out var handler))
        {
            ((Action<T>)handler)?.Invoke(message);
        }
    }
}

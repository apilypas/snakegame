using SnakeGame.Core.Entities;

namespace SnakeGame.Core.Events;

public class NotifyTimerChangedEvent(float timer) : NotifyEvent(null, null, NotifyEventType.TimerChanged)
{
    public float Timer { get; private set; } = timer;
}

public class NotifyEvent(Entity target, Entity triggeredBy, NotifyEventType eventType)
{
    public Entity Target { get; private set; } = target;
    public Entity TriggeredBy { get; private set; } = triggeredBy;
    public NotifyEventType EventType { get; private set; } = eventType;
}

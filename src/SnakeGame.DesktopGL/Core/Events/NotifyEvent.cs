using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core.Events;

public class NotifyTimerChangedEvent(float timer) : NotifyEvent(null, null, NotifyEventType.TimerChanged)
{
    public float Timer { get; private set; } = timer;
}

public class NotifyEvent(EntityBase target, EntityBase triggeredBy, NotifyEventType eventType)
{
    public EntityBase Target { get; private set; } = target;
    public EntityBase TriggeredBy { get; private set; } = triggeredBy;
    public NotifyEventType EventType { get; private set; } = eventType;
}

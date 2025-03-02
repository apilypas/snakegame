using SnakeGame.DesktopGL.Core.Entities;

namespace SnakeGame.DesktopGL.Core.Events;

public class NotifyEvent
{
    public EntityBase Target { get; private set; }
    public EntityBase TriggeredBy { get; private set; }
    public NotifyEventType EventType { get; private set; }

    public NotifyEvent(EntityBase target, EntityBase triggeredBy, NotifyEventType eventType)
    {
        Target = target;
        TriggeredBy = triggeredBy;
        EventType = eventType;
    }
}

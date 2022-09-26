using Parcels.Domain;
using System;
using System.Text.Json;

namespace Parcels.EventEntities;

public record BaseEvent : IBaseEvent
{
    public BaseEvent()
    {
        // Required by EF
    }

    public BaseEvent(EventType eventType, object @event, Guid streamId)
    {
        StreamId = streamId;
        EventType = eventType;
        Value = JsonSerializer.Serialize(@event);
        CreateDate = DateTime.Now;
    }

    public Guid Id { get; init; }
    public Guid StreamId { get; init; }
    public long Sequence { get; init; }
    public int Version { get; set; }
    public EventType EventType { get; init; }
    public string Value { get; init; }
    public DateTime CreateDate { get; init; }
    public bool IsApplied { get; private set; }

    public void MarkAsApplied() => IsApplied = true;

    public T ToTypedEvent<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value);
    }
}

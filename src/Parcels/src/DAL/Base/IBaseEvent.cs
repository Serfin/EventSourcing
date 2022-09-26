using Parcels.Domain;
using System;

namespace Parcels.EventEntities;

public interface IBaseEvent
{
    Guid Id { get; }
    EventType EventType { get; }
    string Value { get; }
    public long Sequence { get; }
    public int Version { get; }
    DateTime CreateDate { get; }
}

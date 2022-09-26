using Parcels.EventEntities;
using System.Collections.Generic;

namespace Parcels.Domain.Abstract;

public interface IAggregate
{
    void Enqueue(BaseEvent @event);
    IEnumerable<BaseEvent> Dequeue();
    void When(BaseEvent @event);
}

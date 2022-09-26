using Parcels.Domain.Abstract;
using Parcels.EventEntities;
using System.Collections.Generic;
using System.Linq;

namespace Parcels.Domain;

public class Aggregate : IAggregate
{
    private readonly Queue<BaseEvent> uncommittedEvents = new Queue<BaseEvent>();

    public virtual void When(object @event) { }

    public void Enqueue(BaseEvent @event)
    {
        uncommittedEvents.Enqueue(@event);
    }

    public IEnumerable<BaseEvent> Dequeue()
    {
        var dequeuedEvents = uncommittedEvents.ToList();

        uncommittedEvents.Clear();

        return dequeuedEvents;
    }

    public virtual void When(BaseEvent @event)
    {
        throw new System.NotImplementedException();
    }
}
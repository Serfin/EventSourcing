using Parcels.Domain.Abstract;
using Parcels.EventEntities;
using System;

namespace Parcels.Domain;

public class Parcel : Aggregate, IEntity
{
    public Guid Id { get; set; }
    public ParcelStatus ParcelStatus { get; private set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string CustomerFirstName { get; private set; }
    public string CustomerLastName { get; private set; }

    public Parcel() { }

    public override void When(BaseEvent @event)
    {
        switch (@event.EventType)
        {
            case EventType.ParcelCreated:
                Apply(@event.ToTypedEvent<ParcelCreated>(@event.Value));
                break;
            case EventType.ParcelStatusUpdated:
                Apply(@event.ToTypedEvent<ParcelStatusUpdated>(@event.Value));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(@event.EventType));
        };

        @event.MarkAsApplied();
    }

    public Parcel(string customerFirstName, string customerLastName)
    {
        var parcelId = Guid.NewGuid();
        var @event = new ParcelCreated(parcelId, customerFirstName, customerLastName);

        Enqueue(new BaseEvent(EventType.ParcelCreated, @event, parcelId));
        Apply(@event);
    }

    public void UpdateParcelStatus(ParcelStatus parcelStatus)
    {
        var @event = new ParcelStatusUpdated(parcelStatus);

        Enqueue(new BaseEvent(EventType.ParcelStatusUpdated, @event, Id));
        Apply(@event);
    }

    public void Apply(ParcelCreated @event)
    {
        Id = @event.Id;
        ParcelStatus = ParcelStatus.Created;
        CustomerFirstName = @event.CustomerFirstName;
        CustomerLastName = @event.CustomerLastName;
    }

    public void Apply(ParcelStatusUpdated @event)
    {
        ParcelStatus = @event.ParcelStatus;
    }
}

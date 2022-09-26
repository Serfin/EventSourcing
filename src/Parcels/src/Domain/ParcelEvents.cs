using Parcels.EventEntities;
using System;

namespace Parcels.Domain;

public enum EventType
{
    None = 0,
    ParcelCreated,
    ParcelStatusUpdated
}

public record ParcelCreated(
    Guid Id,
    string CustomerFirstName,
    string CustomerLastName);

public record ParcelStatusUpdated(
    ParcelStatus ParcelStatus);
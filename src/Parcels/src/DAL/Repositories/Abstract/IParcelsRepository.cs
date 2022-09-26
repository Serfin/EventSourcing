using Parcels.Domain.Abstract;
using Parcels.EventEntities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Parcels.DAL.Repositories.Abstract;

public interface IParcelsRepository
{
    Task<T> AggregateStream<T>(Guid streamId, CancellationToken cancellationToken)
        where T : IAggregate, new();
    Task AppendEvents(Guid streamId, IEnumerable<BaseEvent> events, CancellationToken cancellationToken);
    Task SaveCurrentState(CancellationToken cancellationToken);
    Task<ParcelProjection> GetParcel(Guid parcelId, CancellationToken cancellationToken);
    Task<IEnumerable<ParcelProjection>> GetParcels(CancellationToken cancellationToken);
}

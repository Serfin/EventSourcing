using Microsoft.EntityFrameworkCore;
using Parcels.DAL.Repositories.Abstract;
using Parcels.Domain;
using Parcels.Domain.Abstract;
using Parcels.EventEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Parcels.DAL;

public class ParcelsRepository : IParcelsRepository
{
    private readonly ParcelsContext _parcelsContext;

    public ParcelsRepository(ParcelsContext parcelsContext)
    {
        _parcelsContext = parcelsContext;
    }

    public async Task<T> AggregateStream<T>(Guid streamId, CancellationToken cancellationToken)
        where T : IAggregate, new()
    {
        var @events = await _parcelsContext.Events
            .AsNoTracking()
            .Where(x => x.StreamId == streamId)
            .OrderBy(x => x.Version)
            .ToListAsync(cancellationToken);

        if (!@events.Any()) return default;

        var aggregate = new T();
        foreach (var rawEvent in @events.Where(x => !x.IsApplied))
        {
            aggregate.When(rawEvent);
        }

        return aggregate;
    }

    public async Task AppendEvents(Guid streamId, IEnumerable<BaseEvent> events, CancellationToken cancellationToken)
    {
        if (!events.Any()) return;

        var lastVersion = (await _parcelsContext.Events
            .OrderByDescending(x => x.Version)
            .FirstOrDefaultAsync(x => x.StreamId == streamId, cancellationToken))
            ?.Version ?? 0;

        var versionedEvents = events.Select((x, i) =>
        {
            x.Version = lastVersion + i + 1;

            return x;
        });

        await _parcelsContext.Events.AddRangeAsync(versionedEvents, cancellationToken);
    }

    public async Task<ParcelProjection> GetParcel(Guid parcelId, CancellationToken cancellationToken)
    {
        return await _parcelsContext.Parcels
            .FirstOrDefaultAsync(x => x.Id == parcelId, cancellationToken);
    }

    public async Task<IEnumerable<ParcelProjection>> GetParcels(CancellationToken cancellationToken)
    {
        return await _parcelsContext.Parcels
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task SaveCurrentState(CancellationToken cancellationToken)
    {
        await _parcelsContext.SaveChangesAsync(cancellationToken);
    }
}
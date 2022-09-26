using Microsoft.EntityFrameworkCore;
using Parcels.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Parcels.DAL.Projections;

public class ParcelInlineProjection : IProjection<Parcel>
{
    private readonly ParcelsContext _context;

    public ParcelInlineProjection(ParcelsContext context)
    {
        _context = context;
    }

    public async Task UpsertProjection(Parcel entity, CancellationToken ct = default)
    {
        var projection = await _context.Parcels.FirstOrDefaultAsync(x => x.Id == entity.Id, ct);
        if (projection is null)
        {
            projection = new ParcelProjection
            {
                Id = entity.Id,
                ParcelStatus = entity.ParcelStatus,
                CustomerFirstName = entity.CustomerFirstName,
                CustomerLastName = entity.CustomerLastName,
                CreateDate = DateTime.Now,
            };

            await _context.AddAsync(projection, ct);

            return;
        }

        projection.ParcelStatus = entity.ParcelStatus;
        projection.CustomerFirstName = entity.CustomerFirstName;
        projection.CustomerLastName = entity.CustomerLastName;
        projection.UpdateDate = DateTime.Now;
    }
}

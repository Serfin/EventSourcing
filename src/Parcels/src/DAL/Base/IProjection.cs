using Parcels.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Parcels;

public interface IProjection<T>
{
    Task UpsertProjection(Parcel entity, CancellationToken ct = default);
}

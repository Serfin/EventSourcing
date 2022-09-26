using MediatR;
using Parcels.DAL;
using Parcels.DAL.Repositories.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Parcels.Queries;

public record GetParcelDetailsQuery(Guid ParcelId) : IRequest<ParcelProjection>
{
    public class Handler : IRequestHandler<GetParcelDetailsQuery, ParcelProjection>
    {
        private readonly IParcelsRepository _parcelsRepository;

        public Handler(IParcelsRepository parcelsRepository)
        {
            _parcelsRepository = parcelsRepository;
        }

        public async Task<ParcelProjection> Handle(GetParcelDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _parcelsRepository.GetParcel(request.ParcelId, cancellationToken);
        }
    }
}

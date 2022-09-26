using MediatR;
using Parcels.DAL;
using Parcels.DAL.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Parcels.Queries;

public class GetParcelsQuery : IRequest<IEnumerable<ParcelProjection>>
{
    public class Handler : IRequestHandler<GetParcelsQuery, IEnumerable<ParcelProjection>>
    {
        private readonly IParcelsRepository _parcelsRepository;

        public Handler(IParcelsRepository parcelsRepository)
        {
            _parcelsRepository = parcelsRepository;
        }

        public async Task<IEnumerable<ParcelProjection>> Handle(GetParcelsQuery request, CancellationToken cancellationToken)
        {
            return await _parcelsRepository.GetParcels(cancellationToken);
        }
    }
}

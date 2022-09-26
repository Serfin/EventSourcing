using MediatR;
using Parcels.DAL.Repositories.Abstract;
using Parcels.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Parcels.Commands;

public sealed record CreateParcelCommand(string CustomerFirstName, string CustomerLastName) : IRequest
{
    public sealed class Handler : IRequestHandler<CreateParcelCommand>
    {
        private readonly IParcelsRepository _parcelsRepository;
        private readonly IProjection<Parcel> _parcelProjection;

        public Handler(
            IParcelsRepository parcelsRepository, 
            IProjection<Parcel> parcelProjection)
        {
            _parcelsRepository = parcelsRepository;
            _parcelProjection = parcelProjection;
        }

        public async Task<Unit> Handle(CreateParcelCommand request, CancellationToken cancellationToken)
        {
            var parcel = new Parcel(request.CustomerFirstName, request.CustomerLastName);

            await _parcelsRepository.AppendEvents(parcel.Id, parcel.Dequeue(), cancellationToken);
            await _parcelProjection.UpsertProjection(parcel, cancellationToken);
            await _parcelsRepository.SaveCurrentState(cancellationToken);

            return Unit.Value;
        }
    }
}
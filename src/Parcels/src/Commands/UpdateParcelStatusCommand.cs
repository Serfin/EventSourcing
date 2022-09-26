using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parcels.DAL.Repositories.Abstract;
using Parcels.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Parcels.Commands;

public record UpdateParcelStatusCommand(Guid Id, ParcelStatus ParcelStatus) : IRequest<object>
{
    public class Handler : IRequestHandler<UpdateParcelStatusCommand, object>
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

        public async Task<object> Handle(UpdateParcelStatusCommand request, CancellationToken cancellationToken)
        {
            var parcel = await _parcelsRepository.AggregateStream<Parcel>(request.Id, cancellationToken);
            if (parcel is null) return null;

            parcel.UpdateParcelStatus(request.ParcelStatus);

            await _parcelsRepository.AppendEvents(request.Id, parcel.Dequeue(), cancellationToken);
            await _parcelProjection.UpsertProjection(parcel, cancellationToken);
            await _parcelsRepository.SaveCurrentState(cancellationToken);

            return Unit.Value;      
        }
    }
}

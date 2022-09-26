using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parcels.Commands;
using Parcels.Domain;
using Parcels.Queries;
using System;
using System.Threading.Tasks;

namespace Parcels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParcelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Ok(await _mediator.Send(new CreateParcelCommand("Test 1", "Test 2")));
        }

        [HttpPatch("{parcelId:guid}")]
        public async Task<IActionResult> Update(Guid parcelId, ParcelStatus parcelStatus)
        {
            var result = await _mediator.Send(new UpdateParcelStatusCommand(parcelId, parcelStatus));

            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetParcelsQuery()));
        }
    }
}

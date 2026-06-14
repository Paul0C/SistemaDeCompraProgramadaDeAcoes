using CompraProgramada.Application.ClientContext.Commands.ProductAdherence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompraProgramada.API.Controllers.ClientContext;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("Adhesion")]
    public async Task<IActionResult> Adhesion(ProductAdherenceCommand command)
    {
        var client = await mediator.Send(command);
        return client.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, client)
            : BadRequest(client);
    }
}
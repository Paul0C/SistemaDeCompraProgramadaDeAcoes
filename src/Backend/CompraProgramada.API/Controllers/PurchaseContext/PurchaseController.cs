using CompraProgramada.Application.PurchaseContext.Commands.ExecutePurchase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompraProgramada.API.Controllers.PurchaseContext;

[ApiController]
[Route("api/[controller]")]
public class PurchaseController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("execute-purchase")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> ExecutePurchase([FromBody] ExecutePurchaseCommand command)
    {
        var purchaseResult = await mediator.Send(command);
        return Ok(purchaseResult);
    }
}
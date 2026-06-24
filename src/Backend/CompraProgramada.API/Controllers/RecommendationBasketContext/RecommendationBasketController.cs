using CompraProgramada.Application.RecommendationBasketContext.Commands.CreateRecommendationBasket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompraProgramada.API.Controllers.RecommendationBasketContext;

[ApiController]
[Route("api/admin/basket")]
public class RecommendationBasketController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateRecommendationBasketCommand command)
    {
        var recommendationBasket = await mediator.Send(command);
        return recommendationBasket.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, recommendationBasket)
            : BadRequest(recommendationBasket);
    }
}
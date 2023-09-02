using CQRS.Discounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketsMarket.Controllers;

[ApiController]
[Authorize(policy: "admin")]
[Route("[controller]")]
public class DiscountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddDiscount([FromBody] AddDiscountRequest addDiscountRequest)
    {
        if (await _mediator.Send(addDiscountRequest) is {} discount)
            return Created("created", discount);
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDiscount([FromBody] UpdateDiscountRequest updateDiscountHandler)
    {
        if (await _mediator.Send(updateDiscountHandler))
            return Accepted();
        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveDiscount(Guid id)
    {
        var removeDiscountRequest = new RemoveDiscountRequest { Id = id };
        if (await _mediator.Send(removeDiscountRequest))
            return NoContent();
        return BadRequest();
    }
}
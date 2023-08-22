using ConcertTicketsMarketModel.Model.Performers;
using ConcertTicketsMarketWebApp.CQRS.Performers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketsMarketWebApp.Controllers;

[ApiController]
[Authorize(policy: "admin")]
[Route("[controller]")]
public class PerformersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PerformersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetPerformer(Guid id)
    {
        var getPerformerByIdRequest = new GetPerformerByIdRequest { PerformerId = id };
        if (await _mediator.Send(getPerformerByIdRequest) is { } performer)
            return Ok(performer);
        return NotFound();
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetPerformers([FromQuery] GetPerformersRequest getPerformersRequest)
    {
        if (await _mediator.Send(getPerformersRequest) is List<Performer> { Count: > 0 } performers)
            return Ok(performers);
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddPerformer([FromBody] AddPerformerRequest addPerformerRequest)
    {
        if (await _mediator.Send(addPerformerRequest))
            return Created("created", addPerformerRequest.Performer.Id);
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePerformer([FromBody] UpdatePerformerRequest updatePerformerRequest)
    {
        if (await _mediator.Send(updatePerformerRequest))
            return Accepted();
        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> RemovePerformer(Guid id)
    {
        var removePerformerRequest = new RemovePerformerRequest { Id = id };
        if (await _mediator.Send(removePerformerRequest))
            return NoContent();
        return BadRequest();
    }
}
using ConcertTicketsMarketWebApp.CQRS.Concerts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketsMarketWebApp.Controllers;

[ApiController]
[Authorize(policy: "admin")]
[Route("[controller]")]
public class ConcertsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ConcertsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetAllConcerts([FromQuery] GetConcertsRequest criteria)
    {
        var foundConcerts = await _mediator.Send(criteria, CancellationToken.None);
        return Ok(foundConcerts);
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetConcert(Guid id)
    {
        var getConcertByIdRequest = new GetConcertByIdRequest { ConcertId = id };
        if (await _mediator.Send(getConcertByIdRequest, CancellationToken.None) is { } concert)
            return Ok(concert);
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> PostConcert([FromBody] AddConcertRequest addConcertRequest)
    {
        if (await _mediator.Send(addConcertRequest))
            return Created("Created", addConcertRequest.Name);
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateConcert([FromBody] UpdateConcertRequest updateConcertRequest)
    {
        if (await _mediator.Send(updateConcertRequest))
            return Accepted();
        return BadRequest();
    }
    
    [HttpDelete]
    public async Task<IActionResult> RemoveConcert(Guid id)
    {
        var removeConcertRequest = new RemoveConcertRequest { ConcertId = id };
        if (await _mediator.Send(removeConcertRequest))
            return NoContent();
        return BadRequest();
    }
}
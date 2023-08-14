using ConcertTicketsMarketModel;
using ConcertTicketsMarketWebApp.CQRS.Tickets;
using Duende.IdentityServer.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketsMarketWebApp.Controllers;

[ApiController]
[Authorize(policy: "admin")]
[Route("[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetTickets([FromQuery] GetTicketsRequest getTicketsRequest)
    {
        return Ok(await _mediator.Send(getTicketsRequest));
    }

    [HttpPost]
    public async Task<IActionResult> AddTicket([FromBody] AddTicketsRequest addTicketsRequest)
    {
        if (await _mediator.Send(addTicketsRequest))
            return Created("successful", addTicketsRequest.TicketTemplate.ConcertId);
        return BadRequest();

    }

    [HttpPut]
    public async Task<IActionResult> BookTicket([FromQuery] Guid id)
    {
        if (User.Identity.GetSubjectId() is not { } userId)
            return Unauthorized();
        var bookTicketRequest = new BookTicketRequest { TicketId = id, BookerId = Guid.Parse(userId) };
        if (await _mediator.Send(bookTicketRequest))
            return Accepted();
        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveTicket(Guid id)
    {
        if (User.Identity.GetSubjectId() is not { } userId)
            return Unauthorized();
        var removeTicketRequest = new RemoveTicketsRequest
        {
            IssuerId = Guid.Parse(userId),
            TicketId = id
        };

        if (await _mediator.Send(removeTicketRequest))
            return NoContent();
        return BadRequest();
    }

    // [HttpPut]
    // public async Task<IActionResult> UpdateTickets([FromBody])
}
﻿using ConcertTicketsMarketModel.Model.Concerts;
using CQRS.Concerts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModels.PostingModels;

namespace ConcertTicketsMarket.Controllers;

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
    public async Task<IActionResult> GetAllConcerts([FromQuery] string? query)
    {
        var foundConcerts = await _mediator
            .Send(new GetConcertsRequest { query = query }, CancellationToken.None);
        return Ok(new { concerts = foundConcerts.Item1, maxPages = foundConcerts.Item2 });
    }

    [HttpGet, AllowAnonymous, Route("{id}")]
    public async Task<IActionResult> GetConcert(Guid id)
    {
        var getConcertByIdRequest = new GetConcertByIdRequest { ConcertId = id };
        if (await _mediator.Send(getConcertByIdRequest, CancellationToken.None) is { } concert)
            return Ok(concert);
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> PostConcert([FromBody] ConcertPostingModel addConcertRequest)
    {
        if (await _mediator.Send(new AddConcertRequest{ NewConcert = addConcertRequest}) is { } concert)
            return Created("Created", concert);
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
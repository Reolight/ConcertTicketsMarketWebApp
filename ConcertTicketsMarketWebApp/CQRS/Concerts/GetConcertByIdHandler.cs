using ConcertTicketsMarketModel.Concerts;
using ConcertTicketsMarketWebApp.Data;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts;

public class GetConcertByIdHandler : IRequestHandler<GetConcertByIdRequest, Concert?>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetConcertByIdHandler> _logger;
    public GetConcertByIdHandler(AppDbContext context, ILogger<GetConcertByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Concert?> Handle(GetConcertByIdRequest request, CancellationToken cancellationToken)
    {
        var concert = await _context.Concerts.FindAsync(request.ConcertId);
        if (concert is null)
            _logger.LogWarning("Concert with Id {ConcertId} is not found", request.ConcertId);
        else
            _logger.LogInformation("Retrieved concert with id {ConcertId}", concert.Id);
        return concert;
    }
}
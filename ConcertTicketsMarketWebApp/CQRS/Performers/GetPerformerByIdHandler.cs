using ConcertTicketsMarketModel.Performers;
using ConcertTicketsMarketWebApp.Data;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Performers;

public class GetPerformerByIdHandler : IRequestHandler<GetPerformerByIdRequest, Performer?>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPerformerByIdHandler> _logger;

    public GetPerformerByIdHandler(AppDbContext context, ILogger<GetPerformerByIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Performer?> Handle(GetPerformerByIdRequest request, CancellationToken cancellationToken)
    {
        var performer = await _context.Performers.FindAsync(request.PerformerId);
        if (performer is not null)
            _logger.LogInformation("Retrieved Performer with id {PerformerId}", request.PerformerId);
        else 
            _logger.LogWarning("Requested Performer with Id {PerformerId} not found", request.PerformerId);
        return performer;
    }
}
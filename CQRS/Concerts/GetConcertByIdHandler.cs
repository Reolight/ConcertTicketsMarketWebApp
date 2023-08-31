using ConcertTicketsMarketModel.Data;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using ViewModels;
using IMapper = MapsterMapper.IMapper;

namespace CQRS.Concerts;

public class GetConcertByIdHandler : IRequestHandler<GetConcertByIdRequest, ConcertSuperficial?>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private readonly ILogger<GetConcertByIdHandler> _logger;
    public GetConcertByIdHandler(AppDbContext context, ILogger<GetConcertByIdHandler> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ConcertSuperficial?> Handle(GetConcertByIdRequest request, CancellationToken cancellationToken)
    {
        var concert = await _context.Concerts.FindAsync(request.ConcertId);
        if (concert is null)
            _logger.LogWarning("Concert with Id {ConcertId} is not found", request.ConcertId);
        else
            _logger.LogInformation("Retrieved concert with id {ConcertId}", concert.Id);
        return concert?.Adapt<ConcertSuperficial>();
    }
}
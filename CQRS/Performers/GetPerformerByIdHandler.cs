using ConcertTicketsMarketModel.Data;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ViewModels;

namespace CQRS.Performers;

public class GetPerformerByIdHandler : IRequestHandler<GetPerformerByIdRequest, PerformerViewModel?>
{
    private readonly AppDbContext _context;
    private readonly ILogger<GetPerformerByIdHandler> _logger;
    private readonly IMapper _mapper;

    public GetPerformerByIdHandler(AppDbContext context, ILogger<GetPerformerByIdHandler> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PerformerViewModel?> Handle(GetPerformerByIdRequest request, CancellationToken cancellationToken)
    {
        var performer = await _context.Performers.ProjectToType<PerformerViewModel>().FirstOrDefaultAsync(cancellationToken: cancellationToken);
        _logger.LogInformation("Retrieved Performer with id {PerformerId}", request.PerformerId);
        return performer;
    }
}
using ConcertTicketsMarketModel.Data;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SorterByCriteria;
using ViewModels;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class GetConcertsHandler : IRequestHandler<
        GetConcertsRequest,
        (IEnumerable<ConcertSuperficial>, int)
    >
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly ILogger<GetConcertsHandler> _logger;
        private readonly IFilterSorterPaginatorService _fsp;
        public GetConcertsHandler(AppDbContext context, ILogger<GetConcertsHandler> logger, IMapper mapper, IFilterSorterPaginatorService fsp)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _fsp = fsp;
        }

        public Task<(IEnumerable<ConcertSuperficial>, int)> Handle(GetConcertsRequest request, CancellationToken cancellationToken)
            => Task.FromResult((_context.Concerts, _fsp).WithJsonQuery(request.query)
                .ApplyFilters()
                .ApplySorting()
                .ApplyProjectingAction(concerts => 
                    concerts.AsNoTracking()
                        .ProjectToType<ConcertSuperficial>())
                .ApplyPagination());
    }
}

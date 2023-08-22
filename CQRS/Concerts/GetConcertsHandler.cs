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
        List<ConcertSuperficial>
    >
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly ILogger<GetConcertsHandler> _logger;
        public GetConcertsHandler(AppDbContext context, ILogger<GetConcertsHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public Task<List<ConcertSuperficial>> Handle(GetConcertsRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Concerts
                .AsQueryable()
                .SortByCriteria(request.Sorting)
                .AsNoTracking();
            var concertsVm = _mapper.From(query)
                .ProjectToType<ConcertSuperficial>()
                .ToList();
            return Task.FromResult(concertsVm);
        }
    }
}

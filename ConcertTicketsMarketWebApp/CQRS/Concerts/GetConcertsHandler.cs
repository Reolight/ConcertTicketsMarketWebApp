using ConcertTicketsMarketModel.Concerts;
using ConcertTicketsMarketWebApp.Data;
using MediatR;
using SorterByCriteria;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class GetConcertsHandler : IRequestHandler<
        GetConcertsRequest,
        List<Concert>
    >
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetConcertsHandler> _logger;
        public GetConcertsHandler(AppDbContext context, ILogger<GetConcertsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<List<Concert>> Handle(GetConcertsRequest request, CancellationToken cancellationToken)
        {
            var concerts = _context.Concerts
                    .AsQueryable()
                    .SortByCriteria(request.Criteria)
                    .ToList();
            return Task.FromResult(concerts);
        }
    }
}

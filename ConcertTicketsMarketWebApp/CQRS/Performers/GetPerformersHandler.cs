using ConcertTicketsMarketModel.Performers;
using ConcertTicketsMarketWebApp.Data;
using MediatR;
using SorterByCriteria;

namespace ConcertTicketsMarketWebApp.CQRS.Performers
{
    public class GetPerformersHandler : IRequestHandler<GetPerformersRequest, List<Performer>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetPerformersHandler> _logger;
        public GetPerformersHandler(AppDbContext context, ILogger<GetPerformersHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public Task<List<Performer>> Handle(GetPerformersRequest request, CancellationToken cancellationToken)
        {
            var performers = _context.Performers.AsQueryable()
                .SortByCriteria(request.SortingCriteria)
                .ToList();
            return Task.FromResult(performers);
        }
    }
}

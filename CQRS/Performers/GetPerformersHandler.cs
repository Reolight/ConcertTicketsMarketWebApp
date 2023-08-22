using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model.Performers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                .AsNoTracking()
                .ToList();
            return Task.FromResult(performers);
        }
    }
}

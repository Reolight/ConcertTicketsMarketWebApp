using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model.Performers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SorterByCriteria;

namespace ConcertTicketsMarketWebApp.CQRS.Performers
{
    public class GetPerformersHandler : IRequestHandler<GetPerformersRequest, (IEnumerable<Performer>, int)>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetPerformersHandler> _logger;
        private readonly IFilterSorterPaginatorService _fsp;
        public GetPerformersHandler(AppDbContext context, ILogger<GetPerformersHandler> logger, IFilterSorterPaginatorService fsp)
        {
            _context = context;
            _logger = logger;
            _fsp = fsp;
        }

        public Task<(IEnumerable<Performer>, int)> Handle(GetPerformersRequest request,
            CancellationToken cancellationToken)
            => Task.FromResult((_context.Performers, _fsp).WithJsonQuery(request.JsonQuery)
                .ApplyFilters()
                .ApplySorting()
                .ApplyAction(q => q.AsNoTracking())
                .ApplyPagination());
    }
}

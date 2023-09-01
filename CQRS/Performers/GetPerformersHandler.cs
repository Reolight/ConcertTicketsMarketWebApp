using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model.Performers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SorterByCriteria;

namespace CQRS.Performers
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
        {
            var tuple = (_context.Performers, _fsp).WithJsonQuery(request.JsonQuery).ApplyFilters();
            tuple = tuple.ApplySorting();
            tuple = tuple.ApplyAction(q => q.AsNoTracking());
            var result = tuple.ApplyPagination();
            return Task.FromResult(result);
        }
    }
}

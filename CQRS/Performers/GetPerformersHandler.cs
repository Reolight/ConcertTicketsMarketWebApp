using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model.Performers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SorterByCriteria;
using ViewModels;

namespace CQRS.Performers
{
    public class GetPerformersHandler : IRequestHandler<GetPerformersRequest, (IEnumerable<PerformerViewModel>, int)>
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

        public Task<(IEnumerable<PerformerViewModel>, int)> Handle(GetPerformersRequest request,
            CancellationToken cancellationToken) =>
            Task.FromResult((_context.Performers, _fsp)
                .WithJsonQuery(request.JsonQuery)
                .ApplyFilters()
                .ApplySorting()
                .ApplyProjectingAction(performers => performers
                    .AsNoTracking()
                    .ProjectToType<PerformerViewModel>())
                .ApplyPagination());
    }
}

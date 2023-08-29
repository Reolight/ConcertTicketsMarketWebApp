using ConcertTicketsMarketModel;
using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model;
using ConcertTicketsMarketWebApp.CQRS.Performers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SorterByCriteria;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class GetTicketsHandler : IRequestHandler<GetTicketsRequest, List<Ticket>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetTicketsHandler> _logger;
        private readonly IFilterSorterPaginatorService _fsp;
        public GetTicketsHandler(AppDbContext context, ILogger<GetTicketsHandler> logger, IFilterSorterPaginatorService fsp)
        {
            _context = context;
            _logger = logger;
            _fsp = fsp;
        }

        private List<Ticket> GetForAnonymousUser(Guid concertId, string jsonQuery)
        {
            return 
                _context.Tickets.Where(ticket => ticket.ConcertId == concertId && ticket.OwnerId == null)
                    .ApplyFilterSorting(_fsp, jsonQuery)
                    .AsNoTracking()
                    .ToList();
        }

        private List<Ticket> GetForAuthenticatedUser(Guid concertId, Guid userId,
            string jsonQuery)
        {
            return _context.Tickets
                .Where(ticket => ticket.ConcertId == concertId &&
                                 (ticket.OwnerId == null || ticket.OwnerId == userId))
                .ApplyFilterSorting(_fsp, jsonQuery)
                .AsNoTracking()
                .ToList();
        }

        private List<Ticket> GetForAdmin(Guid concertId, string jsonQuery)
        {
            return _context.Tickets
                .Where(ticket => ticket.ConcertId == concertId)
                .ApplyFilterSorting(_fsp, jsonQuery)
                .AsNoTracking()
                .ToList();
        }

        public Task<List<Ticket>> Handle(GetTicketsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UserId is null)
                {
                    _logger.LogInformation("Anonymous user issued tickets for concert with Id {ConcertId}",
                        request.ConcertId);
                    return Task.FromResult(GetForAnonymousUser(request.ConcertId, request.JsonQuery));
                }

                if (!request.IsAdmin)
                {
                    _logger.LogInformation("User with Id {UserId} issued tickets for concert with Id {ConcertId}",
                        request.UserId.Value, request.ConcertId);
                    return Task.FromResult(GetForAuthenticatedUser(request.ConcertId, request.UserId.Value,
                        request.JsonQuery));
                }

                _logger.LogInformation("Admin with Id {AdminId} decided to check tickets for concert {ConcertId}",
                    request.UserId.Value, request.ConcertId);
                return Task.FromResult(GetForAdmin(request.ConcertId, request.JsonQuery));
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured upon retrieving tickets:\n{Message}\n{StackTrace}",
                    e.Message, e.StackTrace);
                return Task.FromResult<List<Ticket>>(new());
            }
        }
    }
}

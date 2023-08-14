using ConcertTicketsMarketModel;
using ConcertTicketsMarketWebApp.CQRS.Performers;
using ConcertTicketsMarketWebApp.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SorterByCriteria;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class GetTicketsHandler : IRequestHandler<GetTicketsRequest, List<Ticket>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetTicketsHandler> _logger;
        public GetTicketsHandler(AppDbContext context, ILogger<GetTicketsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        private List<Ticket> GetForAnonymousUser(Guid concertId, List<SortingCriterion> sortingCriteria)
        {
            return 
                _context.Tickets.Where(ticket => ticket.ConcertId == concertId && ticket.OwnerId == null)
                .SortByCriteria(sortingCriteria)
                .AsNoTracking()
                .ToList();
        }

        private List<Ticket> GetForAuthenticatedUser(Guid concertId, Guid userId,
            List<SortingCriterion> sortingCriteria)
        {
            return _context.Tickets
                .Where(ticket => ticket.ConcertId == concertId &&
                                 (ticket.OwnerId == null || ticket.OwnerId == userId))
                .SortByCriteria(sortingCriteria)
                .AsNoTracking()
                .ToList();
        }

        private List<Ticket> GetForAdmin(Guid concertId, List<SortingCriterion> sortingCriteria)
        {
            return _context.Tickets
                .Where(ticket => ticket.ConcertId == concertId)
                .SortByCriteria(sortingCriteria)
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
                    return Task.FromResult(GetForAnonymousUser(request.ConcertId, request.SortingCriteria));
                }

                if (!request.IsAdmin)
                {
                    _logger.LogInformation("User with Id {UserId} issued tickets for concert with Id {ConcertId}",
                        request.UserId.Value, request.ConcertId);
                    return Task.FromResult(GetForAuthenticatedUser(request.ConcertId, request.UserId.Value,
                        request.SortingCriteria));
                }

                _logger.LogInformation("Admin with Id {AdminId} decided to check tickets for concert {ConcertId}",
                    request.UserId.Value, request.ConcertId);
                return Task.FromResult(GetForAdmin(request.ConcertId, request.SortingCriteria));
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

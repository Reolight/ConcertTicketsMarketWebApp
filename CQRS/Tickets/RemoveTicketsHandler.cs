using ConcertTicketsMarketWebApp.CQRS.Performers;
using ConcertTicketsMarketWebApp.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class RemoveTicketsHandler : IRequestHandler<RemoveTicketsRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RemoveTicketsHandler> _logger;
        public RemoveTicketsHandler(AppDbContext context, ILogger<RemoveTicketsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(RemoveTicketsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ticketToDelete = await _context.Tickets.FindAsync(request.TicketId)
                                     ?? throw new NullReferenceException(
                                         $"There is no Ticket with Id {request.TicketId}");
                var concert = await _context.Concerts.FindAsync(ticketToDelete.ConcertId)
                              ?? throw new NullReferenceException(
                                  $"Concert with id {ticketToDelete.ConcertId} does not exist");

                if (concert.StartTime > DateTime.UtcNow)
                    throw new InvalidOperationException("Impossible to delete tickets for passed or ongoing concert");
                if (ticketToDelete.OwnerId != null || ticketToDelete.BookingTime != null)
                    throw new InvalidOperationException(
                        $"Ticket removing denied, because it is owned! Issuer ID {request.IssuerId}");

                _context.Tickets.Remove(ticketToDelete);
                return true;
            }
            catch (InvalidOperationException invOp)
            {
                _logger.LogError("{Message}", invOp.Message);
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError("Error on removing the Ticket with Id {TicketId} occured\n{Message}\n\n{StackTrace}",
                    request.TicketId, e.Message, e.StackTrace);
                return false;
            }
        }
    }
}

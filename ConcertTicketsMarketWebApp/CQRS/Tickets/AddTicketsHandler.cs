using ConcertTicketsMarketWebApp.CQRS.Performers;
using ConcertTicketsMarketWebApp.Data;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class AddTicketsHandler : IRequestHandler<AddTicketsRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddTicketsHandler> _logger;
        public AddTicketsHandler(AppDbContext context, ILogger<AddTicketsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(AddTicketsRequest request, CancellationToken cancellationToken)
        {
            int successfullyAddedTickets = 0;
            try
            {
                for (; successfullyAddedTickets < request.Count; successfullyAddedTickets++)
                {
                    var ticketEntry = await _context.Tickets.AddAsync(request.TicketTemplate, cancellationToken);
                    _logger.LogDebug("Added Ticket with Id {Id}", ticketEntry.Entity.Id);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}\n{StackTrace}",
                    e.Message, e.StackTrace);
                return false;
            }
            finally
            {
                _logger.LogInformation("Finished. Amount of successfully added Tickets: {Count} from {All}",
                    successfullyAddedTickets, request.Count);
            }
        }
    }
}

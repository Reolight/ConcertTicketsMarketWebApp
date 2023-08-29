using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model;
using ConcertTicketsMarketWebApp.CQRS.Performers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class AddTicketsHandler : IRequestHandler<AddTicketsRequest, IEnumerable<Ticket>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddTicketsHandler> _logger;
        public AddTicketsHandler(AppDbContext context, ILogger<AddTicketsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<IEnumerable<Ticket>> Handle(AddTicketsRequest request, CancellationToken cancellationToken)
        {
            int successfullyAddedTickets = 0;
            try
            {
                List<Ticket> successfullyAdded = new List<Ticket>();
                for (; successfullyAddedTickets < request.Count; successfullyAddedTickets++)
                {
                    var ticketEntry = await _context.Tickets.AddAsync(request.TicketTemplate, cancellationToken);
                    _logger.LogDebug("Added Ticket with Id {Id}", ticketEntry.Entity.Id);
                    successfullyAdded.Add(ticketEntry.Entity);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return successfullyAdded;
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}\n{StackTrace}",
                    e.Message, e.StackTrace);
                return ArraySegment<Ticket>.Empty;
            }
            finally
            {
                _logger.LogInformation("Finished. Amount of successfully added Tickets: {Count} from {All}",
                    successfullyAddedTickets, request.Count);
            }
        }
    }
}

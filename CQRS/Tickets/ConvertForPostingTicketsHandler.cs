using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model;
using MediatR;

namespace CQRS.Tickets
{
    public class ConvertForPostingTicketsHandler : IRequestHandler<ConvertForPostingTicketsRequest, bool>
    {
        private readonly AppDbContext _context;

        public ConvertForPostingTicketsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ConvertForPostingTicketsRequest request, CancellationToken cancellationToken)
        {
            int successfullyAddedTickets = 0;
            var concertEntity = await _context.Concerts.FindAsync(request.ConcertId);
            
            List<Ticket> convertedTickets = new List<Ticket>();
            foreach (var template in request.TicketTemplates)
                for (; successfullyAddedTickets < template.Count; successfullyAddedTickets++)
                    convertedTickets.Add(new Ticket
                    {
                        Description = template.Description,
                        Price = template.Price,
                        Concert = concertEntity ?? throw new NullReferenceException("there is no concert attach ticket to")
                    });
            
            await _context.Tickets.AddRangeAsync(convertedTickets, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return successfullyAddedTickets == convertedTickets.Count;
        }
    }
}

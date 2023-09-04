using ConcertTicketsMarketModel.Model;
using MediatR;

namespace CQRS.Tickets
{
    public class ConvertForPostingTicketsHandler : IRequestHandler<ConvertForPostingTicketsRequest, IEnumerable<Ticket>>
    {
        public Task<IEnumerable<Ticket>> Handle(ConvertForPostingTicketsRequest request, CancellationToken cancellationToken)
        {
            int successfullyAddedTickets = 0;
            
            List<Ticket> convertedTickets = new List<Ticket>();
            foreach (var template in request.TicketTemplates)
                for (; successfullyAddedTickets < template.Count; successfullyAddedTickets++)
                    convertedTickets.Add(new Ticket
                    {
                        Description = template.Description,
                        Price = template.Price,
                    });

            return Task.FromResult<IEnumerable<Ticket>>(convertedTickets);
        }
    }
}

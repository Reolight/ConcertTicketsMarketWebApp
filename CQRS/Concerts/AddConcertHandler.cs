using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model.Concerts;
using CQRS.Tickets;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CQRS.Concerts
{
    public class AddConcertHandler : IRequestHandler<AddConcertRequest, Concert?>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddConcertHandler> _logger;
        private readonly IMediator _mediator;
        public AddConcertHandler(AppDbContext context, ILogger<AddConcertHandler> logger, IMediator mediator)
        {
            _mediator = mediator;
            (_context, _logger) = (context, logger);
        }

        public async Task<Concert?> Handle(AddConcertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                
                var concert = request.NewConcert.Adapt<Concert>();
                concert.Performer = await _context.Performers.FindAsync(concert.PerformerId) ?? throw new NullReferenceException();
                var addedConcert = await _context.Concerts.AddAsync(concert, cancellationToken);
                _logger.LogInformation("Concert {NewConcertName} with ID: {EntityId}",
                    addedConcert.Entity.Name, addedConcert.Entity.Id);
                await _context.SaveChangesAsync(cancellationToken);
                
                await _mediator.Send(new ConvertForPostingTicketsRequest
                    {
                        TicketTemplates = request.NewConcert.Tickets,
                        ConcertId = addedConcert.Entity.Id
                    },
                        cancellationToken);
                return addedConcert.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError("New concert has not been added. Error: {Message}\n\n{StackTrace}"
                    ,e.Message, e.StackTrace);
                return null;
            }
        }
    }
}

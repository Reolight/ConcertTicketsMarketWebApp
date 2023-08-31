using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model.Performers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CQRS.Performers
{
    public class AddPerformerHandler : IRequestHandler<AddPerformerRequest, Performer?>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddPerformerHandler> _logger;
        public AddPerformerHandler(AppDbContext context, ILogger<AddPerformerHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
         
        public async Task<Performer?> Handle(AddPerformerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var performerEntry = await _context.Performers.AddAsync(request.Performer, cancellationToken);
                _logger.LogInformation("Added {PerformerType} with name {Name}",
                    request.Performer.GetType().Name, request.Performer.Name);
                await _context.SaveChangesAsync(cancellationToken);
                return performerEntry.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}\n{StackTrace}",
                    e.Message, e.StackTrace);
                return null;
            }
        }
    }
}

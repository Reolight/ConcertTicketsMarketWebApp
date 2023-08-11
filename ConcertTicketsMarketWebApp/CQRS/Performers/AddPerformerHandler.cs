using ConcertTicketsMarketModel.Performers;
using ConcertTicketsMarketWebApp.CQRS.Discounts;
using ConcertTicketsMarketWebApp.Data;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ConcertTicketsMarketWebApp.CQRS.Performers
{
    public class AddPerformerHandler : IRequestHandler<AddPerformerRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddPerformerHandler> _logger;
        public AddPerformerHandler(AppDbContext context, ILogger<AddPerformerHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
         
        public async Task<bool> Handle(AddPerformerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Performers.AddAsync(request.Performer, cancellationToken);
                _logger.LogInformation("Added {PerformerType} with name {Name}",
                    request.Performer.GetType().Name, request.Performer.Name);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}\n{StackTrace}",
                    e.Message, e.StackTrace);
                return false;
            }
        }
    }
}

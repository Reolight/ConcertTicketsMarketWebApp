using ConcertTicketsMarketModel.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CQRS.Performers
{
    public class RemovePerformerHandler : IRequestHandler<RemovePerformerRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RemovePerformerHandler> _logger;
        public RemovePerformerHandler(AppDbContext context, ILogger<RemovePerformerHandler> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<bool> Handle(RemovePerformerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var removedPerformer = await _context.Performers.FindAsync(request.Id)
                                       ?? throw new NullReferenceException(
                                           $"There is no Performer with ID {request.Id}");
                if (_context.Concerts.Any(concert => concert.StartTime + concert.Duration > DateTime.UtcNow))
                    throw new InvalidOperationException(
                        "Impossible to delete Performers while they have scheduled concerts!");
                
                _context.Performers.Remove(removedPerformer);
                _logger.LogInformation("Removed {PerformerType} with Id {Id}",
                    removedPerformer.GetType().Name, removedPerformer.Id);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (NullReferenceException nullRef)
            {
                _logger.LogError("{Message}\n{StackTrace",
                    nullRef.Message, nullRef.StackTrace);
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

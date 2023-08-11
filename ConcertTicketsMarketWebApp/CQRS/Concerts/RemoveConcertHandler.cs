using ConcertTicketsMarketWebApp.Data;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class RemoveConcertHandler : IRequestHandler<RemoveConcertRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RemoveConcertHandler> _logger;

        public RemoveConcertHandler(AppDbContext context, ILogger<RemoveConcertHandler> logger)
            => (_context, _logger) = (context, logger);

        public async Task<bool> Handle(RemoveConcertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var concert = await _context.Concerts.FindAsync(request.ConcertId) ??
                              throw new NullReferenceException($"Concert with ID {request.ConcertId} does not exist");

                if (concert.StartTime + concert.Duration > DateTime.UtcNow)
                    throw new InvalidOperationException("Impossible to delete scheduled concert!");
                
                _context.Concerts.Remove(concert);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (NullReferenceException nullRef)
            {
                _logger.LogWarning("{SelfDescribingMessage}\n\n{StackTrace}", nullRef.Message, nullRef.StackTrace);
                // It doesn't exist, so... what a difference between deleted and not deleted but not existing one?
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Removing Concert with ID {Id} finished with exception: {Message}\n\n{StackTrace}",
                    request.ConcertId, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}

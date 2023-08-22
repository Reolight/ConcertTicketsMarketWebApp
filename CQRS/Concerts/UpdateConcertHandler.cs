using ConcertTicketsMarketWebApp.Data;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConcertTicketsMarketWebApp.CQRS.Concerts
{
    public class UpdateConcertHandler: IRequestHandler<UpdateConcertRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateConcertHandler> _logger;

        public UpdateConcertHandler(AppDbContext context, ILogger<UpdateConcertHandler> logger)
            => (_context, _logger) = (context, logger);
        
        public async Task<bool> Handle(UpdateConcertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (await _context.Concerts.FindAsync(request.Id) is not { } concert)
                    throw new NullReferenceException($"There is no Concert with ID {request.Id}");
                request.Adapt(concert);
                _logger.LogInformation("Updated Concert ID: {ConcertId}", request.Id);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Update Concert failed with ID: {ConcertId}.\n{Message}\n\n{StackTrace}"
                    ,request.Id, e.Message, e.StackTrace);
                return false;
            }
        }
    }
}

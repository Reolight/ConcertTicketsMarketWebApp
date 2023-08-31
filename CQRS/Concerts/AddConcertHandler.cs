using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model.Concerts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CQRS.Concerts
{
    public class AddConcertHandler : IRequestHandler<AddConcertRequest, Concert?>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddConcertHandler> _logger;
        public AddConcertHandler(AppDbContext context, ILogger<AddConcertHandler> logger) 
            => (_context, _logger) = (context, logger);
        
        public async Task<Concert?> Handle(AddConcertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var addedConcert = await _context.Concerts.AddAsync(request, cancellationToken);
                _logger.LogInformation("Concert {NewConcertName} with ID: {EntityId}",
                    request.Name, addedConcert.Entity.Id);
                await _context.SaveChangesAsync(cancellationToken);
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

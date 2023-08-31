using ConcertTicketsMarketModel.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CQRS.Performers
{
    public class UpdatePerformerHandler : IRequestHandler<UpdatePerformerRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdatePerformerHandler> _logger;
        public UpdatePerformerHandler(AppDbContext context, ILogger<UpdatePerformerHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(UpdatePerformerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Performers.Update(request.Performer);
                _logger.LogInformation("Updated {PerformerType} with name {Name}",
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

using ConcertTicketsMarketWebApp.CQRS.Concerts;
using ConcertTicketsMarketWebApp.Data;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Discounts
{
    public class RemoveDiscountHandler : IRequestHandler<RemoveDiscountRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RemoveDiscountHandler> _logger;
        public RemoveDiscountHandler(AppDbContext context, ILogger<RemoveDiscountHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(RemoveDiscountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var discountEntry = await _context.Discounts.FindAsync(request.Id, cancellationToken)
                                    ?? throw new NullReferenceException(
                                        $"Discount with ID {request.Id} has not been found");
                _context.Discounts.Remove(discountEntry);
                _logger.LogInformation("Removed Discount {Id}", discountEntry.Id);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (NullReferenceException nullRef)
            {
                _logger.LogError("{Message}\n{StackTrace}", 
                    nullRef.Message, nullRef.StackTrace);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured upon removing discount with Id {Id}.\n{Message}\n\n{StackTrace}",
                    request.Id, e.Message, e.StackTrace);
                return false;
            }
        }
    }
}

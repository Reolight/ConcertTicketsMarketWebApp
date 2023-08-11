using ConcertTicketsMarketWebApp.Data;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Discounts
{
    public class UpdateDiscountHandler : IRequestHandler<UpdateDiscountRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateDiscountHandler> _logger;
        public UpdateDiscountHandler(AppDbContext context, ILogger<UpdateDiscountHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(UpdateDiscountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Discounts.Update(request);
                _logger.LogInformation("Updated Discount {Id}", request.Id);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured upon update discount with Id{Id}.\n{Message}\n\n{StackTrace}",
                    request.Id, e.Message, e.StackTrace);
                return false;
            }
        }
    }
}

﻿using ConcertTicketsMarketWebApp.Data;
using MediatR;

namespace ConcertTicketsMarketWebApp.CQRS.Discounts
{
    public class AddDiscountHandler : IRequestHandler<AddDiscountRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddDiscountHandler> _logger;
        public AddDiscountHandler(AppDbContext context, ILogger<AddDiscountHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(AddDiscountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var discountEntry = await _context.AddAsync(request, cancellationToken);
                _logger.LogInformation("Added Discount {Id}", discountEntry.Entity.Id);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured upon adding new discount with Promocode {Promo}.\n{Message}\n\n{StackTrace}",
                    request.Promocode, e.Message, e.StackTrace);
                return false;
            }
        }
    }
}
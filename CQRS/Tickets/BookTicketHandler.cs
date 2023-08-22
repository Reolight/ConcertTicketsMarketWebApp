using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketWebApp.CQRS.Performers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConcertTicketsMarketWebApp.CQRS.Tickets
{
    public class BookTicketHandler : IRequestHandler<BookTicketRequest, bool>
    {
        public const int BookingExpirationMinutes = 20; 
        
        private readonly AppDbContext _context;
        private readonly ILogger<BookTicketHandler> _logger;
        public BookTicketHandler(AppDbContext context, ILogger<BookTicketHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<bool> Handle(BookTicketRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // exception if there is no ticket
                if (await _context.Tickets.FindAsync(request.TicketId) is not { } bookingTicket)
                    throw new NullReferenceException($"There is no Ticket with Id {request.TicketId}");

                // exception if ticket is already booked and time is not expired (20 min)
                if (bookingTicket.BookingTime is not null &&
                    bookingTicket.BookingTime + TimeSpan.FromMinutes(BookingExpirationMinutes) > DateTime.UtcNow)
                {
                    throw new InvalidOperationException($"The ticket with ID {request.TicketId} is already booked");
                }

                // exception in case ticket was bought
                if (bookingTicket is { OwnerId: { }, BookingTime: null })
                    throw new InvalidOperationException($"The ticket with Id {request.TicketId} is already bought");


                // the rest cases means that ticket is not booked (or has expired booking time) or haven't been bought
                bookingTicket.OwnerId = request.TicketId;
                bookingTicket.BookingTime = DateTime.UtcNow;
                _logger.LogInformation("Ticket with {Id} is booked by user with id {BookerId}",
                    request.TicketId, request.BookerId);
                return true;
            }
            catch (NullReferenceException nullRef)
            {
                _logger.LogError("{Message}\n{StackTrace}", nullRef.Message, nullRef.StackTrace);
                return false;
            }
            catch (InvalidOperationException invOp)
            {
                _logger.LogWarning("{Message}", invOp.Message);
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError("Unknown error when booking ticket: {Message}\n{StackTrace}", 
                    e.Message, e.StackTrace);
                return false;
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace ConcertTicketsMarketWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

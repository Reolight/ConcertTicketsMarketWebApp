using ConcertTicketsMarketModel.Model;
using ConcertTicketsMarketModel.Model.Concerts;
using ConcertTicketsMarketModel.Model.Performers;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicketsMarketModel.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Concert> Concerts { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        // I am using TPH model provided by default
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Singer> Singers { get; set; }
        
        // The next is for searching by location
        public DbSet<Location> Locations { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Band>()
                .HasMany(band => band.Performers)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}

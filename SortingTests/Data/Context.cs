using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SorterTests.Data
{
    public class Context : DbContext
    {
        public DbSet<Person> Persons { get; set; } = null!;

        public Context(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>();
        }

        public static void SeedData(Context context)
        {
            Random rnd = new Random();
            List<Person> persons = new(1000);
            for (int i = 0; i < 1000; i++)
            {
                var person = new Person
                {
                    Age = rnd.Next(1, 120),
                    Name = NamesSurnames.Names[rnd.Next(NamesSurnames.Names.Count)],
                    Surname = NamesSurnames.Surnames[rnd.Next(NamesSurnames.Surnames.Count)]
                };
                person.Money = person.Age > 12 ? (int)(rnd.Next(500_000) * Math.Exp(person.Age)): 0;
                persons.Add(person);
            }

            context.Persons.AddRange(persons);
            context.SaveChanges();
        }
    }
}

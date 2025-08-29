using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FizzBuzz.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Rule> Rules => Set<Rule>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<SessionNumber> SessionNumbers => Set<SessionNumber>();
        public DbSet<Response> Responses => Set<Response>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Pulls all IEntityTypeConfiguration<T> in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }
}

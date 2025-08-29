using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FizzBuzz.Data.Configuration
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> b)
        {
            b.HasKey(g => g.Id);

            b.Property(g => g.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            b.Property(g => g.Author)
                   .IsRequired()
                   .HasMaxLength(100);

            b.HasIndex(g => g.Name)
                   .IsUnique();

            b.HasMany(g => g.Rules)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

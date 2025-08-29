using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FizzBuzz.Data.Configuration
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> b)
        {
            b.HasKey(s => s.Id);

            b.Property(s => s.DurationSeconds)
                .IsRequired();

            b.Property(s => s.StartedAt)
                .IsRequired();

 
            b.HasOne(s => s.Game)
             .WithMany()               // sessions aren’t on Game in your model (no nav prop)
             .HasForeignKey(s => s.GameId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(s => s.Served)
             .WithOne(sn => sn.Session)
             .HasForeignKey(sn => sn.SessionId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

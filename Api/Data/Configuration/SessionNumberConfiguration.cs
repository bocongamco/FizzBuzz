using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FizzBuzz.Data.Configuration
{
    public class SessionNumberConfiguration : IEntityTypeConfiguration<SessionNumber>
    {
        public void Configure(EntityTypeBuilder<SessionNumber> b)
        {
            b.HasKey(sn => new { sn.SessionId, sn.Number }); // Composite key No dub per ses

            b.Property(sn => sn.Number).IsRequired();

            b.HasOne(sn => sn.Session)
             .WithMany(s => s.Served)
             .HasForeignKey(sn => sn.SessionId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

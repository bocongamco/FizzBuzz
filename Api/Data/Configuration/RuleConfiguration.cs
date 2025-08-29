using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FizzBuzz.Data.Configuration
{
    public class RuleConfiguration : IEntityTypeConfiguration<Rule>
    {
        
        public void Configure(EntityTypeBuilder<Rule> b)
        {
            b.HasKey(r => r.Id);

            b.Property(r => r.Divisor)
                .IsRequired();

            b.Property(r => r.Word)
                .IsRequired()
                .HasMaxLength(50);

            b.Property(r => r.Order)
                .IsRequired();

            b.HasOne(r => r.Game)
             .WithMany(g => g.Rules)
             .HasForeignKey(r => r.GameId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }

}


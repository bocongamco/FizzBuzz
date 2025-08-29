using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FizzBuzz.Data.Configuration
{
    public class ResponseConfiguration : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> b)
        {
            b.HasKey(r => r.Id);

            b.Property(r => r.Number).IsRequired();
            b.Property(r => r.Submitted).IsRequired().HasMaxLength(200);
            b.Property(r => r.Expected).IsRequired().HasMaxLength(200);
            b.Property(r => r.IsCorrect).IsRequired();
            b.Property(r => r.CreatedAt).IsRequired();

            b.HasOne(r => r.Session)
             .WithMany() 
             .HasForeignKey(r => r.SessionId)
             .OnDelete(DeleteBehavior.Cascade);

         
            b.HasIndex(r => new { r.SessionId, r.Number }).IsUnique();
        }
    }
}

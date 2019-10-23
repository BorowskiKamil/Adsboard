using Adsboard.Services.Categories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adsboard.Services.Categories.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.Property(f => f.Id)
                .HasColumnName("id");

            builder.Property(f => f.Email)
                .HasColumnName("email")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);
        }
    }
}
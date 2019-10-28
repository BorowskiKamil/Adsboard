using Adsboard.Services.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adsboard.Services.Users.Data.Mappings
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

            builder.Property(f => f.FullName)
                .HasColumnName("full_name")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(false);
        }
    }
}
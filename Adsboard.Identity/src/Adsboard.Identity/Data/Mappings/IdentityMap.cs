using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adsboard.Identity.Data.Mappings
{    
    public class IdentityMap : IEntityTypeConfiguration<Domain.Identity>
    {
        public void Configure(EntityTypeBuilder<Domain.Identity> builder)
        {
            builder.ToTable("identities");

            builder.Property(f => f.Id)
                .HasColumnName("id");

            builder.Property(f => f.Email)
                .HasColumnName("email")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);

            builder.Property(c => c.Role)
                .HasColumnType("varchar(255)")
                .HasColumnName("role")
                .HasDefaultValue("user")
                .IsRequired(false);

            builder.Property(f => f.PasswordHash)
                .HasColumnType("varchar(255)")
                .HasColumnName("password_hash")
                .IsRequired(false);

            builder.Property(f => f.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired(true);

            builder.Property(f => f.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(true);
        }
    }
}
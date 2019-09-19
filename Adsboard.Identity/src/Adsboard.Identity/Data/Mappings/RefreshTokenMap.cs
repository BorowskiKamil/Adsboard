using Adsboard.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adsboard.Identity.Data.Mappings
{    
    public class RefreshTokenMap : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.Property(f => f.Id)
                .HasColumnName("id");

            builder.Property(f => f.IdentityId)
                .HasColumnName("identity_id")
                .IsRequired(true);

            builder.Property(c => c.Token)
                .HasColumnType("varchar(255)")
                .HasColumnName("token")
                .IsRequired(true);

            builder.Property(f => f.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired(true);

            builder.Property(f => f.RevokedAt)
                .HasColumnName("revoked_at")
                .IsRequired(false);
        }
    }
}
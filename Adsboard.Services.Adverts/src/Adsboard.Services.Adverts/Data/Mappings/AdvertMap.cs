using Adsboard.Services.Adverts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adsboard.Services.Adverts.Data.Mappings
{
    public class AdvertMap : IEntityTypeConfiguration<Advert>
    {
        public void Configure(EntityTypeBuilder<Advert> builder)
        {
            builder.ToTable("adverts");

            builder.Property(f => f.Id)
                .HasColumnName("id");

            builder.Property(f => f.Title)
                .HasColumnName("title")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);

            builder.Property(f => f.Description)
                .HasColumnName("description")
                .HasColumnType("TEXT")
                .IsRequired(true);

            builder.Property(f => f.Description)
                .HasColumnName("description")
                .HasColumnType("TEXT")
                .IsRequired(true);

            builder.Property(f => f.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired(true);

            builder.Property(f => f.Category)
                .HasColumnName("category");

            builder.Property(f => f.Image)
                .HasColumnName("image");
        }
    }
}
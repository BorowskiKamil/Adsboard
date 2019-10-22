using Adsboard.Services.Categories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adsboard.Services.Categories.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.Property(f => f.Id)
                .HasColumnName("id");

            builder.Property(f => f.Name)
                .HasColumnName("name")
                .HasColumnType("VARCHAR(255)")
                .IsRequired(true);

            builder.Property(f => f.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired(true);

            builder.Property(f => f.Creator)
                .HasColumnName("creator_id")
                .IsRequired(true);
        }
    }
}
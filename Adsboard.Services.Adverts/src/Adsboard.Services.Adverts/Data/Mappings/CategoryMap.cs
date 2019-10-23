using Adsboard.Services.Adverts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adsboard.Services.Adverts.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.Property(f => f.Id)
                .HasColumnName("id");

            builder.Property(f => f.Creator)
                .HasColumnName("creator_id")
                .IsRequired(true);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogApi.Service.InfraStructure.EntityConfiguration
{
    class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.Property(ci => ci.Name)
                .HasMaxLength(100);

            //builder.HasOne(ci => ci.CatalogBrand)
            //    .WithMany();

            //builder.HasOne(ci => ci.CatalogType)
              //  .WithMany();

            builder.HasIndex(ci => ci.Name);
        }
    }
}

using CatalogApi.Service.InfraStructure.EntityConfiguration;
using Gee.Core.BaseInfrastructure;
using Microsoft.EntityFrameworkCore;


namespace CatalogApi.InfraStructure
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
       : base(options)
        {
        }
        #region Category
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTemplate> CategoryTemplates { get; set; }
        #endregion
        #region Discounts
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Discount_AppliedToCategories> Discount_AppliedToCategories { get; set; }
        public DbSet<Discount_AppliedToManufacturers> Discount_AppliedToManufacturers { get; set; }
        public DbSet<Discount_AppliedToProduct> discount_AppliedToProducts { get; set; }
        public DbSet<DiscountMapping> DiscountMappings { get; set; }
        public DbSet<DiscountRequirement> DiscountRequirements { get; set; }
        #endregion
        #region Manufacturer
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<ManufacturerTemplate> ManufacturerTemplates { get; set; }
        #endregion
        #region Media
        public DbSet<Download> Downloads { get; set; }
        public DbSet<MediaSettings> MediaSettings { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureBinary> PictureBinaries { get; set; }
        public DbSet<Video> Videos { get; set; }
        #endregion
        #region Products
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategoryMapping> ProductCategoryMappings { get; set; }
        public DbSet<ProductManufacturerMapping> ProductManufacturerMappings { get; set; }
        public DbSet<ProductPictureMapping> ProductPictureMappings { get; set; }
        #endregion
        // DbSet<CatalogSettings> CatalogSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.HasPostgresExtension("vector");
            //builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
            //builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductEntityTypeConfiguration());

            // Add the outbox table to this context
            //builder.UseIntegrationEventLogs();
            base.OnModelCreating(builder);
        }
    }
}

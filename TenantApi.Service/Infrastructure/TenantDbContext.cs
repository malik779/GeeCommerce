using Microsoft.EntityFrameworkCore;
using TenantApi.Service.Infrastructure.Domain;

namespace TenantApi.Service.Infrastructure
{
    public class TenantDbContext:DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
       : base(options)
        {
        }
        public DbSet<Tenant> Tenants { get; set; }
       public DbSet<TenantDetail> TenantDetails { get; set; }
        public DbSet<ThemeDetail> ThemeDetails { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

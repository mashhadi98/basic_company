using Company.Domain.Entities;
using Company.Domain.Entities.Authorization;
using Company.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence;

/// <summary>
/// زمینه پایگاه داده شامل Identity، مجوزهای داینامیک و سایر موجودیت‌ها.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
    public DbSet<ProductGallery> ProductGalleries => Set<ProductGallery>();
    public DbSet<ProductTag> ProductTags => Set<ProductTag>();
    public DbSet<CompanyFeature> CompanyFeatures => Set<CompanyFeature>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<StaticPage> StaticPages => Set<StaticPage>();
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<BlogComment> BlogComments => Set<BlogComment>();

    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<OrderRequest> OrderRequests => Set<OrderRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

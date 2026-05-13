using Company.Application.Abstractions;
using Company.Application.Features.CompanyFeatures.Repositories;
using Company.Application.Features.Products.Repositories;
using Company.Infrastructure.Identity;
using Company.Infrastructure.Persistence;
using Company.Infrastructure.Persistence.Identity;
using Company.Infrastructure.Persistence.Repositories;
using Company.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Company.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// پیاده‌سازی پورت‌ها (دیتابیس، فایل، ایمیل، زمان و …) و Identity با مجوزهای داینامیک.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<IAppInfoService, ConfigurationAppInfoService>();

        services.Configure<IdentitySeedOptions>(configuration.GetSection(IdentitySeedOptions.SectionName));

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.Configure<IdentityOptions>(configuration.GetSection("Identity"));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(12);
        });

        services.RemoveAll<IUserClaimsPrincipalFactory<ApplicationUser>>();
        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, PermissionClaimsPrincipalFactory>();

        services.AddScoped<IUserSecurityStampInvalidator, UserSecurityStampInvalidator>();
        services.AddScoped<IPermissionManagementService, PermissionManagementService>();

        // ثبت ریپوزیتوری‌های محصول
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

        // ثبت ریپوزیتوری ویژگی شرکت
        services.AddScoped<ICompanyFeatureRepository, CompanyFeatureRepository>();

        return services;
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Company.Infrastructure.Persistence;

/// <summary>
/// برای دستورات <c>dotnet ef</c> وقتی پروژهٔ استارت‌آپ با پروژهٔ Infrastructure فرق دارد.
/// </summary>
public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var presentationDir = ResolvePresentationDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(presentationDir)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    private static string ResolvePresentationDirectory()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "Company.Presentation", "appsettings.json");
            if (File.Exists(candidate))
            {
                return Path.Combine(dir.FullName, "Company.Presentation");
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            "Could not locate Company.Presentation/appsettings.json. Run dotnet ef from the solution directory.");
    }
}

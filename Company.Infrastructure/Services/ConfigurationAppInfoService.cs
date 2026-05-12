using Company.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Company.Infrastructure.Services;

public sealed class ConfigurationAppInfoService : IAppInfoService
{
    private readonly IConfiguration _configuration;

    public ConfigurationAppInfoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string ApplicationName =>
        _configuration["Application:Name"] ?? "Company";
}

using Microsoft.Extensions.DependencyInjection;

namespace Company.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// ثبت سرویس‌های لایهٔ Application (use caseها، ولیدیتورها، MediatR و غیره).
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // نمونه: services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);
        return services;
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// ثبت سرویس‌های لایهٔ Application (use caseها، ولیدیتورها، MediatR و غیره).
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // ثبت MediatR
        services.AddMediatR(typeof(ApplicationServiceCollectionExtensions).Assembly);

        // ثبت FluentValidation
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);

        return services;
    }
}

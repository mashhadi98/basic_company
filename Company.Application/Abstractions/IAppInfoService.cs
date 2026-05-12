namespace Company.Application.Abstractions;

/// <summary>
/// اطلاعات نمایشی/پیکربندی برنامه — قرارداد لایهٔ Application، پیاده‌سازی در Infrastructure.
/// </summary>
public interface IAppInfoService
{
    string ApplicationName { get; }
}

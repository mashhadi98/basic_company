namespace Company.Application.Abstractions;

/// <summary>
/// پورت زمان سیستم — پیاده‌سازی در Infrastructure تا قوانین دامنه/کاربرد از DateTime.Static جدا بماند.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

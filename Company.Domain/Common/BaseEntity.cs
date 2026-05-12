namespace Company.Domain.Common;

/// <summary>
/// پایهٔ موجودیت‌های دامنه؛ لایهٔ Domain به هیچ پروژهٔ دیگری وابسته نیست.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}

using Company.Application.Features.StaticPages.DTOs;
using MediatR;

namespace Company.Application.Features.StaticPages.Commands;

/// <summary>
/// کامند ایجاد صفحه ثابت
/// </summary>
public class CreateStaticPageCommand : IRequest<StaticPageDto>
{
    public CreateOrUpdateStaticPageDto PageData { get; set; } = new();
}

/// <summary>
/// کامند ویرایش صفحه ثابت
/// </summary>
public class UpdateStaticPageCommand : IRequest<StaticPageDto>
{
    public Guid PageId { get; set; }
    public CreateOrUpdateStaticPageDto PageData { get; set; } = new();
}

/// <summary>
/// کامند حذف صفحه ثابت
/// </summary>
public class DeleteStaticPageCommand : IRequest<bool>
{
    public Guid PageId { get; set; }
}

/// <summary>
/// کامند تغییر وضعیت انتشار صفحه ثابت
/// </summary>
public class ToggleStaticPagePublishCommand : IRequest<bool>
{
    public Guid PageId { get; set; }
}
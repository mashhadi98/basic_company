using Company.Application.Features.StaticPages.DTOs;
using MediatR;

namespace Company.Application.Features.StaticPages.Queries;

/// <summary>
/// کوئری دریافت همه صفحات ثابت
/// </summary>
public class GetStaticPagesQuery : IRequest<List<StaticPageDto>>
{
    public bool? IsPublished { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

/// <summary>
/// کوئری دریافت صفحه ثابت بر اساس Id
/// </summary>
public class GetStaticPageByIdQuery : IRequest<StaticPageDto?>
{
    public Guid PageId { get; set; }
}

/// <summary>
/// کوئری دریافت صفحه ثابت بر اساس Key
/// </summary>
public class GetStaticPageByKeyQuery : IRequest<StaticPageDto?>
{
    public string Key { get; set; } = string.Empty;
}
using Company.Application.Features.CompanyFeatures.DTOs;
using MediatR;

namespace Company.Application.Features.CompanyFeatures.Queries;

/// <summary>
/// کوئری دریافت همه ویژگی‌های شرکت
/// </summary>
public class GetCompanyFeaturesQuery : IRequest<List<CompanyFeatureDto>>
{
    public bool? IsPublished { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

/// <summary>
/// کوئری دریافت ویژگی شرکت بر اساس Id
/// </summary>
public class GetCompanyFeatureByIdQuery : IRequest<CompanyFeatureDto?>
{
    public Guid FeatureId { get; set; }
}
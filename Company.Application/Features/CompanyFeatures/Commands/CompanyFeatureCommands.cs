using Company.Application.Features.CompanyFeatures.DTOs;
using MediatR;

namespace Company.Application.Features.CompanyFeatures.Commands;

/// <summary>
/// کامند ایجاد ویژگی شرکت
/// </summary>
public class CreateCompanyFeatureCommand : IRequest<CompanyFeatureDto>
{
    public CreateOrUpdateCompanyFeatureDto FeatureData { get; set; } = new();
}

/// <summary>
/// کامند ویرایش ویژگی شرکت
/// </summary>
public class UpdateCompanyFeatureCommand : IRequest<CompanyFeatureDto>
{
    public Guid FeatureId { get; set; }
    public CreateOrUpdateCompanyFeatureDto FeatureData { get; set; } = new();
}

/// <summary>
/// کامند حذف ویژگی شرکت
/// </summary>
public class DeleteCompanyFeatureCommand : IRequest<bool>
{
    public Guid FeatureId { get; set; }
}

/// <summary>
/// کامند تغییر وضعیت انتشار ویژگی شرکت
/// </summary>
public class ToggleCompanyFeaturePublishCommand : IRequest<bool>
{
    public Guid FeatureId { get; set; }
}

/// <summary>
/// کامند مرتب‌سازی ویژگی‌های شرکت
/// </summary>
public class ReorderCompanyFeaturesCommand : IRequest<bool>
{
    public List<ReorderItem> Items { get; set; } = new();

    public class ReorderItem
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }
    }
}
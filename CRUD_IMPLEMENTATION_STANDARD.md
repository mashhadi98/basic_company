# CRUD Implementation Standard Guide

**نسخه**: 1.0  
**تاریخ**: 1403/02/24  
**محیط**: Company ASP.NET Core Clean Architecture

---

## 📋 فهرست مطالب

1. [الگو‌های کلی](#الگوهای-کلی)
2. [ساختار پوشه و نام‌گذاری](#ساختار-پوشه-و-نامگذاری)
3. [Entity و Configuration](#entity-و-configuration)
4. [DTO و Validators](#dto-و-validators)
5. [CQRS Handlers](#cqrs-handlers)
6. [Repository Pattern](#repository-pattern)
7. [ViewModel](#viewmodel)
8. [Controller](#controller)
9. [Views](#views)
10. [Dependency Injection](#dependency-injection)
11. [Migration و Database](#migration-و-database)

---

## الگوهای کلی

### معماری
- **Clean Architecture** با 4 لایه: Domain → Application → Infrastructure → Presentation
- **CQRS Pattern** با MediatR برای جدایی Command و Query
- **Repository Pattern** برای Data Access
- **ViewModel** برای Presentation Layer
- **AdminLTE Theme** برای UI

### CRUD عملیات
```
├── Index          (لیست)
├── Details        (جزئیات)
├── Create/Post    (ایجاد)
├── Edit/Post      (ویرایش)
├── Delete/Post    (حذف)
└── TogglePublish  (تغییر وضعیت - اختیاری)
```

---

## ساختار پوشه و نام‌گذاری

```
Company.Domain/
├── Entities/
│   └── EntityName.cs              # Entity اصلی

Company.Application/
├── Features/
│   └── EntityNames/               # بجای "Products" از "EntityNames" استفاده کنید
│       ├── Commands/
│       │   ├── CreateEntityNameCommand.cs
│       │   ├── UpdateEntityNameCommand.cs
│       │   ├── DeleteEntityNameCommand.cs
│       │   └── ToggleEntityNamePublishCommand.cs
│       ├── Queries/
│       │   ├── GetEntityNamesQuery.cs
│       │   └── GetEntityNameByIdQuery.cs
│       ├── Handlers/
│       │   ├── Commands/
│       │   │   ├── CreateEntityNameCommandHandler.cs
│       │   │   ├── UpdateEntityNameCommandHandler.cs
│       │   │   ├── DeleteEntityNameCommandHandler.cs
│       │   │   └── ToggleEntityNamePublishCommandHandler.cs
│       │   └── Queries/
│       │       ├── GetEntityNamesQueryHandler.cs
│       │       └── GetEntityNameByIdQueryHandler.cs
│       ├── DTOs/
│       │   ├── CreateOrUpdateEntityNameDto.cs
│       │   ├── EntityNameDto.cs
│       │   └── EntityNameResponseDto.cs
│       └── Validators/
│           ├── CreateOrUpdateEntityNameDtoValidator.cs
│           └── ...

Company.Infrastructure/
├── Persistence/
│   ├── Configurations/
│   │   └── EntityNameConfiguration.cs
│   ├── Repositories/
│   │   ├── IEntityNameRepository.cs
│   │   └── EntityNameRepository.cs
│   └── Migrations/
│       └── [DATE]_Add[EntityName]Entity.cs

Company.Presentation/
├── Areas/Admin/
│   ├── Controllers/
│   │   └── EntityNamesController.cs
│   ├── Models/
│   │   └── EntityNameViewModel.cs
│   └── Views/
│       └── EntityNames/
│           ├── Index.cshtml
│           ├── Create.cshtml
│           ├── Edit.cshtml
│           └── Details.cshtml
```

---

## Entity و Configuration

### 1. Entity (Domain Layer)

```csharp
// Company.Domain/Entities/EntityName.cs
using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// شرح کامل موجودیت
/// </summary>
public class EntityName : BaseEntity
{
    /// <summary>
    /// کلید منحصر به فرد (اگر نیاز باشد)
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// شرح کوتاه
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// وضعیت انتشار
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// ترتیب نمایش
    /// </summary>
    public int SortOrder { get; set; }
}
```

### 2. Configuration (Infrastructure Layer)

```csharp
// Company.Infrastructure/Persistence/Configurations/EntityNameConfiguration.cs
using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class EntityNameConfiguration : IEntityTypeConfiguration<EntityName>
{
    public void Configure(EntityTypeBuilder<EntityName> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Key)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .IsRequired();

        builder.Property(e => e.IsPublished)
            .HasDefaultValue(false);

        // ایندکس‌ها
        builder.HasIndex(e => e.Key).IsUnique();
        builder.HasIndex(e => e.IsPublished);
    }
}
```

### 3. DbContext

```csharp
// Company.Infrastructure/Persistence/AppDbContext.cs
public DbSet<EntityName> EntityNames => Set<EntityName>();
```

---

## DTO و Validators

### 1. Create/Update DTO

```csharp
// Company.Application/Features/EntityNames/DTOs/CreateOrUpdateEntityNameDto.cs
namespace Company.Application.Features.EntityNames.DTOs;

public class CreateOrUpdateEntityNameDto
{
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public int SortOrder { get; set; }
}
```

### 2. Response DTO

```csharp
// Company.Application/Features/EntityNames/DTOs/EntityNameDto.cs
namespace Company.Application.Features.EntityNames.DTOs;

public class EntityNameDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

### 3. Validator

```csharp
// Company.Application/Features/EntityNames/Validators/CreateOrUpdateEntityNameDtoValidator.cs
using Company.Application.Features.EntityNames.DTOs;
using FluentValidation;

namespace Company.Application.Features.EntityNames.Validators;

public class CreateOrUpdateEntityNameDtoValidator : AbstractValidator<CreateOrUpdateEntityNameDto>
{
    public CreateOrUpdateEntityNameDtoValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("کلید الزامی است")
            .Length(1, 100).WithMessage("کلید باید بین 1 تا 100 کاراکتر باشد")
            .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("کلید فقط می‌تواند شامل حروف انگلیسی، اعداد، خط تیره و زیرخط باشد");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الزامی است")
            .Length(1, 200).WithMessage("عنوان باید بین 1 تا 200 کاراکتر باشد");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("شرح الزامی است");
    }
}
```

---

## CQRS Handlers

### 1. Commands

```csharp
// Company.Application/Features/EntityNames/Commands/CreateEntityNameCommand.cs
using Company.Application.Features.EntityNames.DTOs;
using MediatR;

namespace Company.Application.Features.EntityNames.Commands;

public class CreateEntityNameCommand : IRequest<EntityNameDto>
{
    public CreateOrUpdateEntityNameDto EntityNameData { get; set; } = null!;
}
```

### 2. Command Handler

```csharp
// Company.Application/Features/EntityNames/Handlers/Commands/CreateEntityNameCommandHandler.cs
using Company.Application.Features.EntityNames.Commands;
using Company.Application.Features.EntityNames.DTOs;
using Company.Domain.Entities;
using Company.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Company.Application.Features.EntityNames.Handlers.Commands;

public class CreateEntityNameCommandHandler : IRequestHandler<CreateEntityNameCommand, EntityNameDto>
{
    private readonly IEntityNameRepository _repository;

    public CreateEntityNameCommandHandler(IEntityNameRepository repository)
    {
        _repository = repository;
    }

    public async Task<EntityNameDto> Handle(CreateEntityNameCommand request, CancellationToken cancellationToken)
    {
        // بررسی منحصر بودن کلید
        var existingEntityName = await _repository.GetByKeyAsync(request.EntityNameData.Key);
        if (existingEntityName != null)
            throw new InvalidOperationException("این کلید قبلاً استفاده شده است");

        var entityName = new EntityName
        {
            Key = request.EntityNameData.Key,
            Title = request.EntityNameData.Title,
            Description = request.EntityNameData.Description,
            IsPublished = request.EntityNameData.IsPublished,
            SortOrder = request.EntityNameData.SortOrder
        };

        await _repository.CreateAsync(entityName);
        await _repository.SaveChangesAsync();

        return new EntityNameDto
        {
            Id = entityName.Id,
            Key = entityName.Key,
            Title = entityName.Title,
            Description = entityName.Description,
            IsPublished = entityName.IsPublished,
            SortOrder = entityName.SortOrder,
            CreatedAt = entityName.CreatedAt
        };
    }
}
```

### 3. Queries

```csharp
// Company.Application/Features/EntityNames/Queries/GetEntityNamesQuery.cs
using Company.Application.Features.EntityNames.DTOs;
using MediatR;

namespace Company.Application.Features.EntityNames.Queries;

public class GetEntityNamesQuery : IRequest<List<EntityNameDto>>
{
    public bool? IsPublished { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 10;
}
```

---

## Repository Pattern

### 1. Interface

```csharp
// Company.Application/Repositories/IEntityNameRepository.cs
using Company.Domain.Entities;

namespace Company.Infrastructure.Persistence.Repositories;

public interface IEntityNameRepository
{
    Task<EntityName?> GetByIdAsync(Guid id);
    Task<EntityName?> GetByKeyAsync(string key);
    Task<List<EntityName>> GetAllAsync(bool? isPublished = null);
    Task CreateAsync(EntityName entityName);
    Task UpdateAsync(EntityName entityName);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
```

### 2. Implementation

```csharp
// Company.Infrastructure/Persistence/Repositories/EntityNameRepository.cs
using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class EntityNameRepository : IEntityNameRepository
{
    private readonly AppDbContext _context;

    public EntityNameRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EntityName?> GetByIdAsync(Guid id)
        => await _context.EntityNames.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<EntityName?> GetByKeyAsync(string key)
        => await _context.EntityNames.FirstOrDefaultAsync(e => e.Key == key);

    public async Task<List<EntityName>> GetAllAsync(bool? isPublished = null)
    {
        var query = _context.EntityNames.AsQueryable();

        if (isPublished.HasValue)
            query = query.Where(e => e.IsPublished == isPublished.Value);

        return await query.OrderBy(e => e.SortOrder).ToListAsync();
    }

    public async Task CreateAsync(EntityName entityName)
        => await _context.EntityNames.AddAsync(entityName);

    public async Task UpdateAsync(EntityName entityName)
        => _context.EntityNames.Update(entityName);

    public async Task DeleteAsync(Guid id)
    {
        var entityName = await GetByIdAsync(id);
        if (entityName != null)
            _context.EntityNames.Remove(entityName);
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
```

---

## ViewModel

```csharp
// Company.Presentation/Areas/Admin/Models/EntityNameViewModel.cs
namespace Company.Presentation.Areas.Admin.Models;

public class EntityNameViewModel
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public int SortOrder { get; set; }
    
    // برای آپلود فایل (اگر نیاز باشد)
    public IFormFile? ImageFile { get; set; }
    public string? Image { get; set; }
}
```

---

## Controller

```csharp
// Company.Presentation/Areas/Admin/Controllers/EntityNamesController.cs
using Company.Application.Features.EntityNames.Commands;
using Company.Application.Features.EntityNames.DTOs;
using Company.Application.Features.EntityNames.Queries;
using Company.Presentation.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class EntityNamesController : Controller
{
    private readonly IMediator _mediator;

    public EntityNamesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// لیست موجودیت‌ها
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(bool? isPublished)
    {
        var query = new GetEntityNamesQuery { IsPublished = isPublished };
        var items = await _mediator.Send(query);
        return View(items);
    }

    /// <summary>
    /// جزئیات موجودیت
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var item = await _mediator.Send(new GetEntityNameByIdQuery { EntityNameId = id });
        return item == null ? NotFound() : View(item);
    }

    /// <summary>
    /// ایجاد موجودیت
    /// </summary>
    [HttpGet]
    public IActionResult Create()
        => View(new EntityNameViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EntityNameViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var dto = new CreateOrUpdateEntityNameDto
            {
                Key = model.Key,
                Title = model.Title,
                Description = model.Description,
                IsPublished = model.IsPublished,
                SortOrder = model.SortOrder
            };

            var result = await _mediator.Send(new CreateEntityNameCommand { EntityNameData = dto });
            return RedirectToAction(nameof(Details), new { id = result.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// ویرایش موجودیت
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var item = await _mediator.Send(new GetEntityNameByIdQuery { EntityNameId = id });
        if (item == null)
            return NotFound();

        return View(new EntityNameViewModel
        {
            Id = item.Id,
            Key = item.Key,
            Title = item.Title,
            Description = item.Description,
            IsPublished = item.IsPublished,
            SortOrder = item.SortOrder
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EntityNameViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var dto = new CreateOrUpdateEntityNameDto
            {
                Key = model.Key,
                Title = model.Title,
                Description = model.Description,
                IsPublished = model.IsPublished,
                SortOrder = model.SortOrder
            };

            await _mediator.Send(new UpdateEntityNameCommand { EntityNameId = id, EntityNameData = dto });
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// حذف موجودیت
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteEntityNameCommand { EntityNameId = id });
        
        if (result)
            TempData["Success"] = "موجودیت با موفقیت حذف شد";
        else
            TempData["Error"] = "موجودیت یافت نشد";

        return RedirectToAction(nameof(Index));
    }
}
```

---

## Views

### AdminLTE Standard Layout

```csharp
// Index.cshtml
<div class="row">
    <div class="col-12">
        <div class="card card-outline card-primary">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h3 class="card-title">لیست موجودیت‌ها</h3>
                <a class="btn btn-primary ms-auto" href="/Admin/EntityNames/Create">
                    <i class="fas fa-plus"></i> ایجاد جدید
                </a>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-bordered table-hover align-middle">
                        <thead>
                            <tr>
                                <th>عنوان</th>
                                <th>وضعیت</th>
                                <th style="width: 240px;">عملیات</th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var item in Model)
                            {
                                <tr>
                                    <td>@item.Title</td>
                                    <td>
                                        <span class="badge bg-@(item.IsPublished ? "success" : "secondary")">
                                            @(item.IsPublished ? "منتشر شده" : "پیش‌نویس")
                                        </span>
                                    </td>
                                    <td>
                                        <a class="btn btn-sm btn-outline-info" href="/Admin/EntityNames/Details/@item.Id">
                                            <i class="fas fa-eye"></i>
                                        </a>
                                        <a class="btn btn-sm btn-outline-primary" href="/Admin/EntityNames/Edit/@item.Id">
                                            <i class="fas fa-edit"></i>
                                        </a>
                                    </td>
                                </tr>
                            }
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
```

---

## Dependency Injection

```csharp
// Company.Infrastructure/DependencyInjection/InfrastructureServiceCollectionExtensions.cs
services.AddScoped<IEntityNameRepository, EntityNameRepository>();
```

---

## Migration و Database

```bash
# ایجاد Migration
dotnet ef migrations add Add[EntityName]Entity --project Company.Infrastructure --startup-project Company.Presentation --no-build

# اعمال Migration
dotnet ef database update --project Company.Infrastructure --startup-project Company.Presentation
```

---

## ✅ Checklist پیاده‌سازی

- [ ] Entity درست شده
- [ ] Configuration نوشته شده
- [ ] DbSet به AppDbContext اضافه شد
- [ ] DTOs ایجاد شدند
- [ ] Validators نوشته شدند
- [ ] Commands/Queries تعریف شدند
- [ ] Handlers پیاده‌سازی شدند
- [ ] Repository Interface و Implementation درست شدند
- [ ] ViewModel ایجاد شد
- [ ] Controller نوشته شد
- [ ] Views (Index, Create, Edit, Details) ایجاد شدند
- [ ] DI Registration انجام شد
- [ ] Migration اعمال شد
- [ ] Controller در Sidebar اضافه شد
- [ ] Testing انجام شد

---

## نکات مهم

✅ **استفاده از ViewModel برای Presentation**  
✅ **استفاده از MediatR برای CQRS**  
✅ **Validation در DTO و Validator**  
✅ **Async/Await برای تمام عملیات DB**  
✅ **Exception Handling در Controller**  
✅ **XML Documentation برای متدها**  
✅ **AdminLTE Bootstrap Classes برای UI**  
✅ **RTL Support برای Persian UI**

---

**نویسنده**: GitHub Copilot  
**آخرین بروزرسانی**: 1403/02/24
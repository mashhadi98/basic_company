using Company.Application.Features.StaticPages.Commands;
using Company.Application.Features.StaticPages.DTOs;
using Company.Application.Features.StaticPages.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.StaticPages.Handlers;

public class CreateStaticPageCommandHandler : IRequestHandler<CreateStaticPageCommand, StaticPageDto>
{
    private readonly IStaticPageRepository _repository;

    public CreateStaticPageCommandHandler(IStaticPageRepository repository)
    {
        _repository = repository;
    }

    public async Task<StaticPageDto> Handle(CreateStaticPageCommand request, CancellationToken cancellationToken)
    {
        var dto = request.PageData;

        // بررسی منحصر به فرد بودن کلید
        var existingPage = await _repository.GetByKeyAsync(dto.Key, cancellationToken);
        if (existingPage != null)
            throw new InvalidOperationException($"صفحه‌ای با کلید '{dto.Key}' قبلاً وجود دارد.");

        var page = new StaticPage
        {
            Key = dto.Key,
            Title = dto.Title,
            Summary = dto.Summary,
            Description = dto.Description,
            Image = dto.Image,
            IsPublished = dto.IsPublished
        };

        await _repository.AddAsync(page, cancellationToken);

        return MapToDto(page);
    }

    private StaticPageDto MapToDto(StaticPage page)
    {
        return new StaticPageDto
        {
            Id = page.Id,
            Key = page.Key,
            Title = page.Title,
            Summary = page.Summary,
            Description = page.Description,
            Image = page.Image,
            IsPublished = page.IsPublished,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            CreatedBy = page.CreatedBy,
            UpdatedBy = page.UpdatedBy
        };
    }
}
using Company.Application.Features.StaticPages.Commands;
using Company.Application.Features.StaticPages.DTOs;
using Company.Application.Features.StaticPages.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.StaticPages.Handlers;

public class UpdateStaticPageCommandHandler : IRequestHandler<UpdateStaticPageCommand, StaticPageDto>
{
    private readonly IStaticPageRepository _repository;

    public UpdateStaticPageCommandHandler(IStaticPageRepository repository)
    {
        _repository = repository;
    }

    public async Task<StaticPageDto> Handle(UpdateStaticPageCommand request, CancellationToken cancellationToken)
    {
        var dto = request.PageData;

        var page = await _repository.GetByIdAsync(request.PageId, cancellationToken);
        if (page == null)
            throw new InvalidOperationException($"صفحه ثابت با شناسه {request.PageId} یافت نشد.");

        // بررسی منحصر به فرد بودن کلید (اگر تغییر کرده)
        if (page.Key != dto.Key)
        {
            var existingPage = await _repository.GetByKeyAsync(dto.Key, cancellationToken);
            if (existingPage != null)
                throw new InvalidOperationException($"صفحه‌ای با کلید '{dto.Key}' قبلاً وجود دارد.");
        }

        page.Key = dto.Key;
        page.Title = dto.Title;
        page.Summary = dto.Summary;
        page.Description = dto.Description;
        page.Image = dto.Image;
        page.IsPublished = dto.IsPublished;

        await _repository.UpdateAsync(page, cancellationToken);

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
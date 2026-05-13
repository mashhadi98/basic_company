using Company.Application.Features.StaticPages.DTOs;
using Company.Application.Features.StaticPages.Queries;
using Company.Application.Features.StaticPages.Repositories;
using MediatR;

namespace Company.Application.Features.StaticPages.Handlers;

public class GetStaticPagesQueryHandler : IRequestHandler<GetStaticPagesQuery, List<StaticPageDto>>
{
    private readonly IStaticPageRepository _repository;

    public GetStaticPagesQueryHandler(IStaticPageRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<StaticPageDto>> Handle(GetStaticPagesQuery request, CancellationToken cancellationToken)
    {
        var pages = await _repository.GetAllAsync(request.IsPublished, request.Skip, request.Take, cancellationToken);

        return pages.Select(MapToDto).ToList();
    }

    private StaticPageDto MapToDto(Domain.Entities.StaticPage page)
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
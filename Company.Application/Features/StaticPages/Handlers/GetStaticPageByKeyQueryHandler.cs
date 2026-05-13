using Company.Application.Features.StaticPages.DTOs;
using Company.Application.Features.StaticPages.Queries;
using Company.Application.Features.StaticPages.Repositories;
using MediatR;

namespace Company.Application.Features.StaticPages.Handlers;

public class GetStaticPageByKeyQueryHandler : IRequestHandler<GetStaticPageByKeyQuery, StaticPageDto?>
{
    private readonly IStaticPageRepository _repository;

    public GetStaticPageByKeyQueryHandler(IStaticPageRepository repository)
    {
        _repository = repository;
    }

    public async Task<StaticPageDto?> Handle(GetStaticPageByKeyQuery request, CancellationToken cancellationToken)
    {
        var page = await _repository.GetByKeyAsync(request.Key, cancellationToken);
        if (page == null)
            return null;

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
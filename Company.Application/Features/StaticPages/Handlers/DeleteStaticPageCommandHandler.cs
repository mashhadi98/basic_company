using Company.Application.Features.StaticPages.Commands;
using Company.Application.Features.StaticPages.Repositories;
using MediatR;

namespace Company.Application.Features.StaticPages.Handlers;

public class DeleteStaticPageCommandHandler : IRequestHandler<DeleteStaticPageCommand, bool>
{
    private readonly IStaticPageRepository _repository;

    public DeleteStaticPageCommandHandler(IStaticPageRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteStaticPageCommand request, CancellationToken cancellationToken)
    {
        var page = await _repository.GetByIdAsync(request.PageId, cancellationToken);
        if (page == null)
            return false;

        await _repository.DeleteAsync(request.PageId, cancellationToken);
        return true;
    }
}
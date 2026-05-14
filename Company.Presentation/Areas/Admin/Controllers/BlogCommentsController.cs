using Company.Application.Features.Blog.Commands;
using Company.Application.Features.Blog.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BlogCommentsController : Controller
{
    private readonly IMediator _mediator;

    public BlogCommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(Guid? postId, bool? isApproved)
    {
        var query = new GetBlogCommentsQuery
        {
            PostId = postId,
            IsApproved = isApproved
        };

        var comments = await _mediator.Send(query);
        return View(comments);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _mediator.Send(new ApproveBlogCommentCommand { CommentId = id });
        TempData[result ? "Success" : "Error"] = result ? "کامنت با موفقیت تایید شد." : "کامنت یافت نشد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(Guid id)
    {
        var result = await _mediator.Send(new RejectBlogCommentCommand { CommentId = id });
        TempData[result ? "Success" : "Error"] = result ? "کامنت حذف شد." : "کامنت یافت نشد.";
        return RedirectToAction(nameof(Index));
    }
}

namespace MovieLibrary.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MovieLibrary.Common;
    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Comments;

    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [Authorize]
        public IActionResult Create(int id)
        {
            var viewModel = new InputCommentViewModel
            {
                Id = id,
            };
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(InputCommentViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                if (this.commentsService.CheckForОbsceneWords(model.Content) == false)
                {
                    var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    await this.commentsService.CreateCommentAsync(model, userId);
                    return this.Redirect($"/Comments/AllComments?id={model.Id}");
                }
                else
                {
                    this.TempData["Message"] = "Your comment contents obscene words!";
                    var viewModel = new InputCommentViewModel
                    {
                        Id = model.Id,
                    };
                    return this.View(viewModel);
                }
            }
        }

        [Authorize]
        public IActionResult AllComments(int id, int page = 1)
        {
            const int ItemPerPage = 7;
            var viewModel = this.commentsService.GetAllComment(id, page, ItemPerPage);
            viewModel.ItemsPerPage = ItemPerPage;
            viewModel.PageNumber = page;
            viewModel.Count = this.commentsService.GetCommentsCount(id);
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult All(int id = 1)
        {
            const int ItemPerPage = 7;
            var viewModel = this.commentsService.GetAllComment(id, ItemPerPage);
            viewModel.ItemsPerPage = ItemPerPage;
            viewModel.PageNumber = id;
            viewModel.Count = this.commentsService.GetAllCommentsCount();
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int commentId, int movieId)
        {
            await this.commentsService.DeleteCommentAsync(commentId);
            return this.Redirect($"/Movies/Details?id={movieId}");
        }
    }
}

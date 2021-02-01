namespace MovieLibrary.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Comments;

    public class CommentsController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly ICommentsService commentsService;

        public CommentsController(IUsersService usersService, ICommentsService commentsService)
        {
            this.usersService = usersService;
            this.commentsService = commentsService;
        }

        public IActionResult CreateComment(int movieId)
        {
            var viewModel = new InputCommentViewModel
            {
                MovieId = movieId,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(InputCommentViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                var userEmail = this.User.FindFirstValue(ClaimTypes.Email);
                var userId = this.usersService.GetUserId(userEmail);
                await this.commentsService.CreateCommentAsync(model, userId);
                return this.Redirect($"/Comments/AllComments?movieId={model.MovieId}");
            }
        }

        public IActionResult AllComments(int movieId)
        {
            var viewModel = this.commentsService.GetAllComment(movieId);
            return this.View(viewModel);
        }

        public IActionResult All()
        {
            var viewModel = this.commentsService.GetAllComment();
            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(int commentId, int movieId)
        {
            await this.commentsService.DeleteCommentAsync(commentId);
            return this.Redirect($"/Comments/AllComments?movieId={movieId}");
        }
    }
}

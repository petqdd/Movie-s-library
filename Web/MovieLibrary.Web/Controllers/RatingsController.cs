namespace MovieLibrary.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Ratings;

    public class RatingsController : BaseController
    {
        private readonly IRatingsService ratingsService;
        private readonly IUsersService usersService;

        public RatingsController(IRatingsService ratingsService, IUsersService usersService)
        {
            this.ratingsService = ratingsService;
            this.usersService = usersService;
        }

        public IActionResult Index(OutputRatingsViewModel model, int movieId)
        {
            var userEmail = this.User.FindFirstValue(ClaimTypes.Email);
            var userId = this.usersService.GetUserId(userEmail);
            if (this.ratingsService.IsUserVote(model, userId))
            {
                model.IsVoted = true;
            }
            else
            {
                model.IsVoted = false;
                model.MovieId = movieId;
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(OutputRatingsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                var userEmail = this.User.FindFirstValue(ClaimTypes.Email);
                var userId = this.usersService.GetUserId(userEmail);
                var viewModel = new InputCreateRatingViewModel
                {
                    MovieId = model.MovieId,
                    UserId = userId,
                    Rating = model.Rating,
                };

                await this.ratingsService.AddVoteAsync(viewModel);
                return this.Redirect($"/Ratings/Success?movieId={viewModel.MovieId}");
            }
        }

        public IActionResult Success(int movieId)
        {
            return this.View(movieId);
        }
    }
}

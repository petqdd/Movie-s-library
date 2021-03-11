namespace MovieLibrary.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Services.Data;
    using MovieLibrary.Web.ViewModels.Ratings;

    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : BaseController
    {
        private readonly IRatingsService ratingsService;

        public RatingsController(IRatingsService ratingsService)
        {
            this.ratingsService = ratingsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostRatingResponseModel>> Post(PostRatingInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var model = new InputCreateRatingViewModel
            {
                UserId = userId,
                MovieId = input.MovieId,
                Rating = input.Value,
            };
            await this.ratingsService.SetVoteAsync(model);
            var averageVote = this.ratingsService.CalculateUserRating(input.MovieId);
            return new PostRatingResponseModel { AverageRating = averageVote };
        }
    }
}

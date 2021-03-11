namespace MovieLibrary.Services.Data
{
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Ratings;

    public interface IRatingsService
    {
        Task SetVoteAsync(InputCreateRatingViewModel model);

        double CalculateUserRating(int movieId);
    }
}

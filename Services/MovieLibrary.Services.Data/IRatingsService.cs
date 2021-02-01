namespace MovieLibrary.Web.Services
{
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Ratings;

    public interface IRatingsService
    {
        Task AddVoteAsync(InputCreateRatingViewModel model);

        bool IsUserVote(OutputRatingsViewModel model, string userId);
    }
}

namespace MovieLibrary.Web.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Ratings;

    public class RatingsService : IRatingsService
    {
        private readonly IDeletableEntityRepository<Rating> ratingsRepository;
        private readonly IRepository<UsersRatings> usersRatingsRepository;
        private readonly IRepository<MoviesRatings> moviesRatingsRepository;

        public RatingsService(
                    IDeletableEntityRepository<Rating> ratingsRepository,
                    IRepository<UsersRatings> usersRatingsRepository,
                    IRepository<MoviesRatings> moviesRatingsRepository)
        {
            this.ratingsRepository = ratingsRepository;
            this.usersRatingsRepository = usersRatingsRepository;
            this.moviesRatingsRepository = moviesRatingsRepository;
        }

        public bool IsUserVote(OutputRatingsViewModel model, string userId)
        {
            var result = this.ratingsRepository
                                .AllAsNoTracking()
                                .Where(x => x.Movies.Any(y => y.MovieId == model.MovieId)
                                          && x.Users.Any(y => y.UserId == userId))
                               .FirstOrDefault();
            return result != null ? true : false;
        }

        public async Task AddVoteAsync(InputCreateRatingViewModel model)
        {
            var currentRating = new Rating
            {
                Vote = model.Rating,
                CreatedData = DateTime.UtcNow,
            };
            await this.ratingsRepository.AddAsync(currentRating);
            await this.ratingsRepository.SaveChangesAsync();
            var ratingId = currentRating.Id;

            var movieRating = new MoviesRatings
            {
                MovieId = model.MovieId,
                RatingId = ratingId,
            };

            var userRating = new UsersRatings
            {
                UserId = model.UserId,
                RatingId = ratingId,
            };
            await this.moviesRatingsRepository.AddAsync(movieRating);
            await this.usersRatingsRepository.AddAsync(userRating);
            await this.ratingsRepository.SaveChangesAsync();
        }
    }
}

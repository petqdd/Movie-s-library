namespace MovieLibrary.Web.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Services.Data;
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

        public async Task SetVoteAsync(InputCreateRatingViewModel model)
        {
            var isExistingRating = this.ratingsRepository
                                .AllAsNoTracking()
                                .Where(x => x.Movies.Any(y => y.MovieId == model.MovieId)
                                          && x.Users.Any(y => y.UserId == model.UserId))
                               .FirstOrDefault();
            var currentRating = new Rating
            {
                Vote = model.Rating,
                CreatedData = DateTime.UtcNow,
            };
            await this.ratingsRepository.AddAsync(currentRating);
            await this.ratingsRepository.SaveChangesAsync();
            var ratingId = currentRating.Id;

            if (isExistingRating == null)
            {
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
            else
            {
                var currentUserRating = this.usersRatingsRepository.AllAsNoTracking()
                    .Where(x => x.UserId == model.UserId && x.RatingId == isExistingRating.Id)
                    .FirstOrDefault();

                currentUserRating.RatingId = ratingId;
                this.usersRatingsRepository.Update(currentUserRating);
                await this.usersRatingsRepository.SaveChangesAsync();

                var currentMovieRating = this.moviesRatingsRepository.AllAsNoTracking()
                    .Where(x => x.MovieId == model.MovieId && x.RatingId == isExistingRating.Id)
                    .FirstOrDefault();

                currentMovieRating.RatingId = ratingId;
                this.moviesRatingsRepository.Update(currentMovieRating);
                await this.moviesRatingsRepository.SaveChangesAsync();

                var rating = this.ratingsRepository.AllAsNoTracking()
                    .FirstOrDefault(x => x.Id == isExistingRating.Id);

                this.ratingsRepository.Delete(rating);
                await this.ratingsRepository.SaveChangesAsync();
            }
        }

        public double CalculateUserRating(int movieId)
        {
            int usersVoteCont = this.moviesRatingsRepository
                                  .AllAsNoTracking()
                                  .Where(x => x.MovieId == movieId)
                                  .Count();
            double usersVoteSum = this.moviesRatingsRepository
                                    .AllAsNoTracking()
                                    .Where(x => x.MovieId == movieId)
                                    .Sum(x => x.Rating.Vote);

            if (usersVoteCont == 0)
            {
                return 0;
            }

            var userRating = Math.Round(usersVoteSum / usersVoteCont, 1);
            return userRating;
        }
    }
}

namespace MovieLibrary.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Movies;

    public class MoviesService : IMoviesService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<Artist> artistsRepository;
        private readonly IRepository<UsersMovie> usersMoviesRepository;
        private readonly IRepository<MoviesRatings> moviesRatingsRepository;

        public MoviesService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Artist> artistsRepository,
            IRepository<UsersMovie> usersMoviesRepository,
            IRepository<MoviesRatings> moviesRatingsRepository)
        {
            this.moviesRepository = moviesRepository;
            this.artistsRepository = artistsRepository;
            this.usersMoviesRepository = usersMoviesRepository;
            this.moviesRatingsRepository = moviesRatingsRepository;
        }

        public async Task CreateMovieAsync(InputCreateMovieViewModel model)
        {
            var movie = new Movie
            {
                Name = model.Name,
                Year = model.Year,
                CategoryId = model.CategoryId,
                Runtime = model.Runtime,
                Storyline = model.Storyline,
                PosterPath = model.PosterPath,
                TrailerUrl = model.TrailerUrl,
                ImdbRating = model.ImdbRating,
                CreatedDate = DateTime.UtcNow,
                UserRating = 0,
            };

            foreach (var artist in model.Artists)
            {
                var currentArtist = this.artistsRepository
                                        .AllAsNoTracking()
                                        .FirstOrDefault(x => x.Name == artist.Name);
                if (currentArtist == null)
                {
                    currentArtist = new Artist { Name = artist.Name };
                }

                movie.Artists.Add(new MoviesArtist
                {
                    Artist = currentArtist,
                });
            }

            await this.moviesRepository.AddAsync(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public ICollection<OutputMovieViewModel> GetAllMovies()
        {
            var movies = this.moviesRepository
                             .AllAsNoTracking()
                             .Where(x => x.IsDeleted == false)
                             .Select(x => new OutputMovieViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Year = x.Year,
                                 Runtime = x.Runtime,
                                 ImdbRating = x.ImdbRating,
                                 TrailerUrl = x.TrailerUrl,
                                 PosterPath = x.PosterPath,
                                 Storyline = x.Storyline,
                                 Category = x.Category.Name,
                                 UserRating = CalculateUserRating(x.Id, this.moviesRatingsRepository),
                             })
                             .ToList();
            foreach (var movie in movies)
            {
                var currentMovie = this.moviesRepository
                                       .AllAsNoTracking()
                                       .Where(x => x.Id == movie.Id)
                                       .FirstOrDefault();

                currentMovie.UserRating = movie.UserRating;
            }

            this.moviesRepository.SaveChangesAsync();
            return movies;
        }

        public bool IsMovieCollected(int movieId, string userId)
        {
            var result = this.usersMoviesRepository
                             .AllAsNoTracking()
                             .Where(x => x.MovieId == movieId && x.UserId == userId)
                             .FirstOrDefault();
            return result != null ? true : false;
        }

        public DetailsMovieViewModel Details(int movieId)
        {
            var movie = this.moviesRepository
                            .AllAsNoTracking()
                            .Where(x => x.Id == movieId)
                            .Select(x => new
                            {
                                x.PosterPath,
                                x.Storyline,
                                x.Id,
                                x.Artists,
                                x.Comments,
                            })
                            .FirstOrDefault();

            var detail = new DetailsMovieViewModel
            {
                Id = movie.Id,
                Storyline = movie.Storyline,
                PosterUrl = movie.PosterPath,
            };

            var artists = new List<Artist>();
            foreach (var artist in movie.Artists)
            {
                var currentArtist = this.artistsRepository
                                        .AllAsNoTracking()
                                        .FirstOrDefault(x => x.Id == artist.ArtistId);
                artists.Add(currentArtist);
            }

            detail.Artists = artists;
            return detail;
        }

        public async Task AddFilmToUserCollectionAsync(string userId, int movieId)
        {
            if (this.usersMoviesRepository.All().Any(x => x.MovieId == movieId && x.UserId == userId))
            {
                return;
            }

            var userMovie = new UsersMovie
            {
                MovieId = movieId,
                UserId = userId,
            };

            await this.usersMoviesRepository.AddAsync(userMovie);
            await this.usersMoviesRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCollectionAsync(string userId, int movieId)
        {
            var userMovie = new UsersMovie
            {
                MovieId = movieId,
                UserId = userId,
            };

            var userMovieId = this.usersMoviesRepository
                .AllAsNoTracking()
                .Where(x => x.MovieId == movieId && x.UserId == userId)
                .Select(x =>x.Id)
                .FirstOrDefault();
            userMovie.Id = userMovieId;
            this.usersMoviesRepository.Delete(userMovie);
            await this.usersMoviesRepository.SaveChangesAsync();
        }

        public ICollection<OutputMovieViewModel> GetAllMoviesInMyCollection(string userId)
        {
            var movies = this.usersMoviesRepository
                             .AllAsNoTracking()
                             .Where(x => x.UserId == userId && x.Movie.IsDeleted == false)
                             .Select(x => new OutputMovieViewModel
                             {
                                 Id = x.Movie.Id,
                                 Name = x.Movie.Name,
                                 Year = x.Movie.Year,
                                 Runtime = x.Movie.Runtime,
                                 ImdbRating = x.Movie.ImdbRating,
                                 TrailerUrl = x.Movie.TrailerUrl,
                                 PosterPath = x.Movie.PosterPath,
                                 Storyline = x.Movie.Storyline,
                                 Category = x.Movie.Category.Name,
                                 UserRating = CalculateUserRating(x.MovieId, this.moviesRatingsRepository),
                             })
                             .ToList();
            return movies;
        }

        public async Task DeleteMovieAsync(int movieId)
        {
            var movie = this.moviesRepository
                            .AllAsNoTracking()
                            .Where(x => x.Id == movieId)
                            .FirstOrDefault();

            this.moviesRepository.Delete(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public async Task EditMovieAsync(int movieId, InputCreateMovieViewModel model)
        {
            var movie = this.moviesRepository
                            .AllAsNoTracking()
                            .Where(x => x.Id == movieId)
                            .FirstOrDefault();
            movie.Name = model.Name;
            movie.Year = model.Year;
            movie.Runtime = model.Runtime;
            movie.PosterPath = model.PosterPath;
            movie.TrailerUrl = model.TrailerUrl;
            movie.ImdbRating = model.ImdbRating;
            movie.Storyline = model.Storyline;
            this.moviesRepository.Update(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        public InputCreateMovieViewModel GetMovieForEdit(int movieId)
        {
            var movie = this.moviesRepository
                            .AllAsNoTracking()
                            .Where(x => x.Id == movieId)
                            .Select(x => new InputCreateMovieViewModel
                            {
                                Name = x.Name,
                                Year = x.Year,
                                Runtime = x.Runtime,
                                PosterPath = x.PosterPath,
                                TrailerUrl = x.TrailerUrl,
                                ImdbRating = x.ImdbRating,
                                Storyline = x.Storyline,
                            })
                            .FirstOrDefault();
            return movie;
        }

        public ICollection<OutputMovieViewModel> GetAllMoviesInCategory(string category)
        {
            var movies = this.moviesRepository
                             .AllAsNoTracking()
                             .Where(x => x.Category.Name == category)
                             .Select(x => new OutputMovieViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Year = x.Year,
                                 Runtime = x.Runtime,
                                 ImdbRating = x.ImdbRating,
                                 TrailerUrl = x.TrailerUrl,
                                 PosterPath = x.PosterPath,
                                 Storyline = x.Storyline,
                                 Category = x.Category.Name,
                                 UserRating = CalculateUserRating(x.Id, this.moviesRatingsRepository),
                             })
                             .ToList();
            if (category == "all")
            {
                movies = this.moviesRepository
                             .AllAsNoTracking()
                             .Select(x => new OutputMovieViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Year = x.Year,
                                 Runtime = x.Runtime,
                                 ImdbRating = x.ImdbRating,
                                 TrailerUrl = x.TrailerUrl,
                                 PosterPath = x.PosterPath,
                                 Storyline = x.Storyline,
                                 Category = x.Category.Name,
                                 UserRating = x.UserRating,
                             })
                             .ToList();
            }

            return movies;
        }

        public bool IsMovieExisting(string movieName)
        {
            return this.moviesRepository
                       .AllAsNoTracking()
                       .Any(x => x.Name == movieName);
        }

        public double CalculateTotalUserRating(int movieId)
        {
            return CalculateUserRating(movieId, this.moviesRatingsRepository);
        }

        private static double CalculateUserRating(int movieId, IRepository<MoviesRatings> moviesRatingsRepository)
        {
            int usersVoteCont = moviesRatingsRepository
                                  .AllAsNoTracking()
                                  .Where(x => x.MovieId == movieId)
                                  .Count();
            double usersVoteSum = moviesRatingsRepository
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

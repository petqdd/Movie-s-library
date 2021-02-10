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
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Director> directorsRepository;
        private readonly IRepository<UsersMovie> usersMoviesRepository;
        private readonly IRepository<MoviesRatings> moviesRatingsRepository;
        private readonly IRepository<MoviesCategory> moviesCategoriesRepository;
        private readonly IRepository<MoviesArtist> moviesArtisRepository;

        public MoviesService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Artist> artistsRepository,
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Director> directorsRepository,
            IRepository<UsersMovie> usersMoviesRepository,
            IRepository<MoviesRatings> moviesRatingsRepository,
            IRepository<MoviesCategory> moviesCategoriesRepository,
            IRepository<MoviesArtist> moviesArtisRepository)
        {
            this.moviesRepository = moviesRepository;
            this.artistsRepository = artistsRepository;
            this.categoriesRepository = categoriesRepository;
            this.directorsRepository = directorsRepository;
            this.usersMoviesRepository = usersMoviesRepository;
            this.moviesRatingsRepository = moviesRatingsRepository;
            this.moviesCategoriesRepository = moviesCategoriesRepository;
            this.moviesArtisRepository = moviesArtisRepository;
        }

        public async Task CreateMovieAsync(InputCreateMovieViewModel model)
        {
            var movie = new Movie
            {
                Name = model.Name,
                Year = model.Year,
                Runtime = model.Runtime,
                Storyline = model.Storyline,
                PosterPath = model.PosterPath,
                SecondPosterPath = model.SecondPosterPath,
                TrailerUrl = model.TrailerUrl,
                ImdbRating = model.ImdbRating,
                CreatedDate = DateTime.UtcNow,
                UserRating = 0,
            };
            movie.Director.Name = model.Director;
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

            foreach (var category in model.Categories)
            {
                var currentCategory = this.categoriesRepository
                                        .AllAsNoTracking()
                                        .FirstOrDefault(x => x.Name == category.CategoryName);
                if (currentCategory == null)
                {
                    currentCategory = new Category { Name = category.CategoryName };
                }

                movie.Categories.Add(new MoviesCategory
                {
                    Category = currentCategory,
                });
            }

            await this.moviesRepository.AddAsync(movie);
            await this.moviesRepository.SaveChangesAsync();
        }

        //public ICollection<OutputMovieViewModel> GetAllMovies()
        //{
        //    var movies = this.moviesRepository
        //                     .AllAsNoTracking()
        //                     .Where(x => x.IsDeleted == false)
        //                     .Select(x => new OutputMovieViewModel
        //                     {
        //                         Id = x.Id,
        //                         Name = x.Name,
        //                         Year = x.Year,
        //                         PosterPath = x.PosterPath,
        //                         //Runtime = x.Runtime,
        //                         //ImdbRating = x.ImdbRating,
        //                         //TrailerUrl = x.TrailerUrl,
        //                         //PosterPath = x.PosterPath,
        //                         //Storyline = x.Storyline,
        //                         //UserRating = CalculateUserRating(x.Id, this.moviesRatingsRepository),
        //                         Categories = x.Categories.Select(x => x.Category.Name).ToList(),
        //                     })
        //                     .ToList();
        //    //foreach (var movie in movies)
        //    //{
        //    //    var currentMovie = this.moviesRepository
        //    //                           .AllAsNoTracking()
        //    //                           .Where(x => x.Id == movie.Id)
        //    //                           .FirstOrDefault();

        //    //    currentMovie.UserRating = movie.UserRating;
        //    //}

        //    this.moviesRepository.SaveChangesAsync();
        //    return movies;
        //}

        //public bool IsMovieCollected(int movieId, string userId)
        //{
        //    var result = this.usersMoviesRepository
        //                     .AllAsNoTracking()
        //                     .Where(x => x.MovieId == movieId && x.UserId == userId)
        //                     .FirstOrDefault();
        //    return result != null ? true : false;
        //}

        //public DetailsMovieViewModel Details(int movieId)
        //{
        //    var movie = this.moviesRepository
        //                    .AllAsNoTracking()
        //                    .Where(x => x.Id == movieId)
        //                    .FirstOrDefault();

        //    var details = new DetailsMovieViewModel
        //    {
        //        PosterUrl = movie.PosterPath,
        //        Storyline = movie.Storyline,
        //        Id = movie.Id,
        //        Name = movie.Name,
        //        Year = movie.Year,
        //        TrailerUrl = movie.TrailerUrl,
        //        Runtime = movie.Runtime,
        //        UserRating = movie.UserRating,
        //        ImdbRating = movie.ImdbRating,
        //        SecondPosterUrl = movie.SecondPosterPath,
        //    };

        //    details.Director = this.directorsRepository.AllAsNoTracking()
        //                                      .Where(x => x.Id == movie.DirectorId)
        //                                      .Select(x => x.Name)
        //                                      .FirstOrDefault();

        //    var currentArtists = this.moviesArtisRepository.AllAsNoTracking()
        //                    .Where(x => x.MovieId == details.Id)
        //                    .Select(x => x.Artist.Name)
        //                    .ToList();

        //    foreach (var artist in currentArtists)
        //    {
        //        details.Artists.Add(new Artist { Name = artist });
        //    }

        //    var currentCategories = this.moviesCategoriesRepository.AllAsNoTracking()
        //        .Where(x => x.MovieId == details.Id)
        //        .Select(x => x.Category.Name)
        //        .ToList();
        //    details.Categories = string.Join(',', currentCategories);

        //    return details;
        //}

        //public async Task AddFilmToUserCollectionAsync(string userId, int movieId)
        //{
        //    if (this.usersMoviesRepository.All().Any(x => x.MovieId == movieId && x.UserId == userId))
        //    {
        //        return;
        //    }

        //    var userMovie = new UsersMovie
        //    {
        //        MovieId = movieId,
        //        UserId = userId,
        //    };

        //    await this.usersMoviesRepository.AddAsync(userMovie);
        //    await this.usersMoviesRepository.SaveChangesAsync();
        //}

        //public async Task RemoveFromCollectionAsync(string userId, int movieId)
        //{
        //    var userMovie = new UsersMovie
        //    {
        //        MovieId = movieId,
        //        UserId = userId,
        //    };

        //    var userMovieId = this.usersMoviesRepository
        //        .AllAsNoTracking()
        //        .Where(x => x.MovieId == movieId && x.UserId == userId)
        //        .Select(x => x.Id)
        //        .FirstOrDefault();
        //    userMovie.Id = userMovieId;
        //    this.usersMoviesRepository.Delete(userMovie);
        //    await this.usersMoviesRepository.SaveChangesAsync();
        //}

        //public ICollection<OutputMovieViewModel> GetAllMoviesInMyCollection(string userId)
        //{
        //    var movies = this.usersMoviesRepository
        //                     .AllAsNoTracking()
        //                     .Where(x => x.UserId == userId && x.Movie.IsDeleted == false)
        //                     .Select(x => new OutputMovieViewModel
        //                     {
        //                         Id = x.Movie.Id,
        //                         Name = x.Movie.Name,
        //                         Year = x.Movie.Year,
        //                         //Runtime = x.Movie.Runtime,
        //                         //ImdbRating = x.Movie.ImdbRating,
        //                         //TrailerUrl = x.Movie.TrailerUrl,
        //                         //PosterPath = x.Movie.PosterPath,
        //                         //Storyline = x.Movie.Storyline,
        //                         //Categories = x.Movie.Categories.Select(x => x.Category.Name).ToList(),
        //                         //UserRating = CalculateUserRating(x.MovieId, this.moviesRatingsRepository),
        //                     })
        //                     .ToList();
        //    return movies;
        //}

        //public async Task DeleteMovieAsync(int movieId)
        //{
        //    var movie = this.moviesRepository
        //                    .AllAsNoTracking()
        //                    .Where(x => x.Id == movieId)
        //                    .FirstOrDefault();

        //    this.moviesRepository.Delete(movie);
        //    await this.moviesRepository.SaveChangesAsync();
        //}

        //public async Task EditMovieAsync(int movieId, InputCreateMovieViewModel model)
        //{
        //    var movie = this.moviesRepository
        //                    .AllAsNoTracking()
        //                    .Where(x => x.Id == movieId)
        //                    .FirstOrDefault();
        //    movie.Name = model.Name;
        //    movie.Year = model.Year;
        //    movie.Runtime = model.Runtime;
        //    movie.PosterPath = model.PosterPath;
        //    movie.TrailerUrl = model.TrailerUrl;
        //    movie.ImdbRating = model.ImdbRating;
        //    movie.Storyline = model.Storyline;
        //    this.moviesRepository.Update(movie);
        //    await this.moviesRepository.SaveChangesAsync();
        //}

        //public InputCreateMovieViewModel GetMovieForEdit(int movieId)
        //{
        //    var movie = this.moviesRepository
        //                    .AllAsNoTracking()
        //                    .Where(x => x.Id == movieId)
        //                    .Select(x => new InputCreateMovieViewModel
        //                    {
        //                        Name = x.Name,
        //                        Year = x.Year,
        //                        Runtime = x.Runtime,
        //                        PosterPath = x.PosterPath,
        //                        TrailerUrl = x.TrailerUrl,
        //                        ImdbRating = x.ImdbRating,
        //                        Storyline = x.Storyline,
        //                    })
        //                    .FirstOrDefault();
        //    return movie;
        //}

        //public ICollection<OutputMovieViewModel> GetAllMoviesInCategory(string category)
        //{
        //    var movies = this.moviesRepository
        //                     .AllAsNoTracking()
        //                     .Where(x => x.Categories.Select(x => x.Category.Name).Contains(category))
        //                     .Select(x => new OutputMovieViewModel
        //                     {
        //                         Id = x.Id,
        //                         Name = x.Name,
        //                         Year = x.Year,
        //                         //Runtime = x.Runtime,
        //                         //ImdbRating = x.ImdbRating,
        //                         //TrailerUrl = x.TrailerUrl,
        //                         //PosterPath = x.PosterPath,
        //                         //Storyline = x.Storyline,
        //                         //Categories = x.Categories.Select(x => x.Category.Name).ToList(),
        //                         //UserRating = CalculateUserRating(x.Id, this.moviesRatingsRepository),
        //                     })
        //                     .ToList();
        //    if (category == "all")
        //    {
        //        movies = this.moviesRepository
        //                     .AllAsNoTracking()
        //                     .Select(x => new OutputMovieViewModel
        //                     {
        //                         Id = x.Id,
        //                         Name = x.Name,
        //                         Year = x.Year,
        //                         //Runtime = x.Runtime,
        //                         //ImdbRating = x.ImdbRating,
        //                         //TrailerUrl = x.TrailerUrl,
        //                         //PosterPath = x.PosterPath,
        //                         //Storyline = x.Storyline,
        //                         //Categories = x.Categories.Select(x => x.Category.Name).ToList(),
        //                         //UserRating = x.UserRating,
        //                     })
        //                     .ToList();
        //    }

        //    return movies;
        //}

        //public bool IsMovieExisting(string movieName)
        //{
        //    return this.moviesRepository
        //               .AllAsNoTracking()
        //               .Any(x => x.Name == movieName);
        //}

        //public double CalculateTotalUserRating(int movieId)
        //{
        //    return CalculateUserRating(movieId, this.moviesRatingsRepository);
        //}

        //private static double CalculateUserRating(int movieId, IRepository<MoviesRatings> moviesRatingsRepository)
        //{
        //    int usersVoteCont = moviesRatingsRepository
        //                          .AllAsNoTracking()
        //                          .Where(x => x.MovieId == movieId)
        //                          .Count();
        //    double usersVoteSum = moviesRatingsRepository
        //                            .AllAsNoTracking()
        //                            .Where(x => x.MovieId == movieId)
        //                            .Sum(x => x.Rating.Vote);

        //    if (usersVoteCont == 0)
        //    {
        //        return 0;
        //    }

        //    var userRating = Math.Round(usersVoteSum / usersVoteCont, 1);
        //    return userRating;
        //}
    }
}

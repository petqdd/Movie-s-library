namespace MovieLibrary.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Services.Data;
    using MovieLibrary.Services.Mapping;
    using MovieLibrary.Web.ViewModels.Movies;

    public class MoviesService : IMoviesService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<Artist> artistsRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Director> directorsRepository;
        private readonly IRepository<UsersMovie> usersMoviesRepository;
        private readonly IRepository<MoviesArtist> moviesArtistsRepository;
        private readonly IRepository<MoviesCategory> moviesCategoriesRepository;
        private readonly IRepository<MoviesArtist> moviesArtisRepository;
        private readonly IRatingsService ratingsService;

        public MoviesService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Artist> artistsRepository,
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Director> directorsRepository,
            IRepository<UsersMovie> usersMoviesRepository,
            IRepository<MoviesArtist> moviesArtistsRepository,
            IRepository<MoviesCategory> moviesCategoriesRepository,
            IRepository<MoviesArtist> moviesArtisRepository,
            IRatingsService ratingsService)
        {
            this.moviesRepository = moviesRepository;
            this.artistsRepository = artistsRepository;
            this.categoriesRepository = categoriesRepository;
            this.directorsRepository = directorsRepository;
            this.usersMoviesRepository = usersMoviesRepository;
            this.moviesArtistsRepository = moviesArtistsRepository;
            this.moviesCategoriesRepository = moviesCategoriesRepository;
            this.moviesArtisRepository = moviesArtisRepository;
            this.ratingsService = ratingsService;
        }

        public async Task CreateMovieAsync(InputCreateMovieViewModel model)
        {
            var movieExists = this.moviesRepository
                    .AllAsNoTracking()
                    .Any(x => x.Name == model.Name && x.IsDeleted == false);
            if (!movieExists)
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

                var currentDirector = this.directorsRepository.AllAsNoTracking().Where(x => x.Name == model.Director).FirstOrDefault();
                if (currentDirector == null)
                {
                    var newDirector = new Director { Name = model.Director };
                    await this.directorsRepository.AddAsync(newDirector);
                    await this.directorsRepository.SaveChangesAsync();
                    movie.DirectorId = newDirector.Id;
                }
                else
                {
                    movie.DirectorId = currentDirector.Id;
                }

                await this.moviesRepository.AddAsync(movie);
                await this.moviesRepository.SaveChangesAsync();

                foreach (var artist in model.Artists)
                {
                    var currentArtist = this.artistsRepository
                                            .AllAsNoTracking()
                                            .FirstOrDefault(x => x.Name == artist.Name);
                    if (currentArtist == null)
                    {
                        currentArtist = new Artist { Name = artist.Name };
                        await this.artistsRepository.AddAsync(currentArtist);
                        await this.artistsRepository.SaveChangesAsync();
                    }

                    var movieArtist = new MoviesArtist
                    {
                        ArtistId = currentArtist.Id,
                        MovieId = movie.Id,
                    };
                    await this.moviesArtistsRepository.AddAsync(movieArtist);
                    await this.moviesArtistsRepository.SaveChangesAsync();
                    var id = movieArtist.Id;
                }

                foreach (var category in model.Categories)
                {
                    var currentCategory = this.categoriesRepository
                                            .AllAsNoTracking()
                                            .FirstOrDefault(x => x.Id == int.Parse(category));
                    var movieCategory = new MoviesCategory
                    {
                        MovieId = movie.Id,
                        CategoryId = currentCategory.Id,
                    };
                    await this.moviesCategoriesRepository.AddAsync(movieCategory);
                    await this.moviesCategoriesRepository.SaveChangesAsync();
                }
            }
        }

        public IEnumerable<T> GetAllMovies<T>(int page, int itemPerPage)
        {
            var movies = this.moviesRepository
                             .AllAsNoTracking()
                             .OrderByDescending(x => x.Id)
                             .Skip((page - 1) * itemPerPage)
                             .Take(itemPerPage)
                             .Where(x => x.IsDeleted == false)
                             .To<T>()
                             //.Select(x => new OutputMovieViewModel
                             //{
                             //    Id = x.Id,
                             //    Name = x.Name,
                             //    Year = x.Year,
                             //    PosterPath = x.PosterPath,
                             //    //Runtime = x.Runtime,
                             //    //ImdbRating = x.ImdbRating,
                             //    //TrailerUrl = x.TrailerUrl,
                             //    //PosterPath = x.PosterPath,
                             //    //Storyline = x.Storyline,
                             //    //UserRating = CalculateUserRating(x.Id, this.moviesRatingsRepository),
                             //    Categories = x.Categories.Select(x => x.Category.Name).ToList(),
                             //})
                             .ToList();
            this.moviesRepository.SaveChangesAsync();
            return movies;
        }

        public int GetMoviesCount()
        {
            return this.moviesRepository.All().Count();
        }

        public int GetMoviesCountInCollection(string userId)
        {
            return this.usersMoviesRepository
                       .All()
                       .Where(x => x.UserId == userId)
                       .Count();
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
                            .FirstOrDefault();

            var details = new DetailsMovieViewModel
            {
                PosterUrl = movie.PosterPath,
                Storyline = movie.Storyline,
                Id = movie.Id,
                Name = movie.Name,
                Year = movie.Year,
                TrailerUrl = movie.TrailerUrl,
                Runtime = movie.Runtime,
                UserRating = movie.UserRating,
                ImdbRating = movie.ImdbRating,
                SecondPosterUrl = movie.SecondPosterPath,
            };

            details.Director = this.directorsRepository.AllAsNoTracking()
                                              .Where(x => x.Id == movie.DirectorId)
                                              .Select(x => x.Name)
                                              .FirstOrDefault();
            details.AverageRating = this.ratingsService.CalculateUserRating(movieId);

            var currentArtists = this.moviesArtisRepository.AllAsNoTracking()
                            .Where(x => x.MovieId == details.Id)
                            .Select(x => x.Artist)
                            .ToList();

            foreach (var artist in currentArtists)
            {
                details.Artists.Add(new Artist
                {
                    Name = artist.Name,
                    BiographyUrl = artist.BiographyUrl,
                    PhotoUrl = artist.PhotoUrl,
                });
            }

            var currentCategories = this.moviesCategoriesRepository.AllAsNoTracking()
                .Where(x => x.MovieId == details.Id)
                .Select(x => x.Category.Name)
                .ToList();
            details.Categories = string.Join(',', currentCategories);

            return details;
        }

        public async Task AddFilmToUserCollectionAsync(string userId, int movieId)
        {
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
                .Select(x => x.Id)
                .FirstOrDefault();
            userMovie.Id = userMovieId;
            this.usersMoviesRepository.Delete(userMovie);
            await this.usersMoviesRepository.SaveChangesAsync();
        }

        public ICollection<OutputMovieViewModel> GetAllMoviesInMyCollection(string userId, int page, int itemPerPage)
        {
            var movies = this.usersMoviesRepository
                             .AllAsNoTracking()
                             .Where(x => x.UserId == userId && x.Movie.IsDeleted == false)
                             .OrderByDescending(x => x.Id)
                             .Skip((page - 1) * itemPerPage)
                             .Take(itemPerPage)
                             .Select(x => new OutputMovieViewModel
                             {
                                 Id = x.Movie.Id,
                                 Name = x.Movie.Name,
                                 Year = x.Movie.Year,
                                 PosterPath = x.Movie.PosterPath,
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

        public InputEditMovieViewModel GetMovieForEdit(int movieId)
        {
            var movie = this.moviesRepository
                                         .AllAsNoTracking()
                                         .Where(x => x.Id == movieId)
                                         .Select(x => new InputEditMovieViewModel
                                         {
                                             Id = x.Id,
                                             Name = x.Name,
                                             Year = x.Year,
                                             Runtime = x.Runtime,
                                             PosterPath = x.PosterPath,
                                             SecondPosterPath = x.SecondPosterPath,
                                             TrailerUrl = x.TrailerUrl,
                                             ImdbRating = x.ImdbRating,
                                             Storyline = x.Storyline,
                                             Director = x.Director.Name,
                                         })
                                         .FirstOrDefault();
            return movie;
        }

        public async Task EditMovieAsync(int id, InputEditMovieViewModel model)
        {
            var movie = this.moviesRepository
                            .AllAsNoTracking()
                            .Where(x => x.Id == id)
                            .FirstOrDefault();
            movie.Name = model.Name;
            movie.Year = model.Year;
            movie.Runtime = model.Runtime;
            movie.PosterPath = model.PosterPath;
            movie.SecondPosterPath = model.SecondPosterPath;
            movie.TrailerUrl = model.TrailerUrl;
            movie.ImdbRating = model.ImdbRating;
            movie.Storyline = model.Storyline;
            this.moviesRepository.Update(movie);
            await this.moviesRepository.SaveChangesAsync();

            var existingMovieCategoryIds = this.moviesCategoriesRepository.AllAsNoTracking()
                .Where(x => x.MovieId == model.Id)
                .Select(x => x.Id)
                .ToArray();

            for (int i = 0; i < existingMovieCategoryIds.Length; i++)
            {
                var currentMovieCategoryForDelete = this.moviesCategoriesRepository
                   .AllAsNoTracking()
                   .Where(x => x.Id == existingMovieCategoryIds[i])
                   .FirstOrDefault();
                this.moviesCategoriesRepository.Delete(currentMovieCategoryForDelete);
                await this.moviesCategoriesRepository.SaveChangesAsync();
            }

            foreach (var category in model.Categories)
            {
                var currentMovieCategory = new MoviesCategory
                {
                    MovieId = model.Id,
                    CategoryId = int.Parse(category),
                };

                await this.moviesCategoriesRepository.AddAsync(currentMovieCategory);
                await this.moviesCategoriesRepository.SaveChangesAsync();
            }
        }

        public ICollection<OutputMovieViewModel> GetAllMoviesInCategory(string category, int page, int itemPerPage)
        {
            var movies = this.moviesRepository
                             .AllAsNoTracking()
                             .Where(x => x.Categories.Select(x => x.Category.Name).Contains(category))
                             .OrderByDescending(x => x.Id)
                             .Skip((page - 1) * itemPerPage)
                             .Take(itemPerPage)
                             .Select(x => new OutputMovieViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Year = x.Year,
                                 PosterPath = x.PosterPath,
                             })
                             .ToList();
            if (category == "all")
            {
                movies = this.moviesRepository
                             .AllAsNoTracking()
                             .OrderByDescending(x => x.Id)
                             .Skip((page - 1) * itemPerPage)
                             .Take(itemPerPage)
                             .Select(x => new OutputMovieViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Year = x.Year,
                                 PosterPath = x.PosterPath,
                             })
                             .ToList();
            }

            return movies;
        }

        public int GetMoviesCountInCategory(string category)
        {
            int count = 0;
            if (category == "all")
            {
                count = this.moviesCategoriesRepository
                            .AllAsNoTracking()
                            .Count();
            }
            else
            {
                count = this.moviesCategoriesRepository
                            .AllAsNoTracking()
                            .Where(x => x.Category.Name == category)
                            .Count();
            }

            return count;
        }

        public ICollection<OutputMovieViewModel> GetTop15MovieImdb()
        {
            var movies = this.moviesRepository.AllAsNoTracking()
                    .Where(x => x.IsDeleted == false)
                    .OrderByDescending(x => x.ImdbRating)
                    .Take(15)
                    .Select(x => new OutputMovieViewModel
                    {
                        Name = x.Name,
                        Year = x.Year,
                        PosterPath = x.PosterPath,
                        Id = x.Id,
                    })
                    .ToList();
            return movies;
        }
    }
}

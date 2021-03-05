namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Movies;

    public class SearchService : ISearchService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<Director> directorsRepository;
        private readonly IRepository<MoviesCategory> moviesCategoriesRepository;
        private readonly IRepository<MoviesArtist> moviesArtisRepository;

        public SearchService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Director> directorsRepository,
            IRepository<MoviesCategory> moviesCategoriesRepository,
            IRepository<MoviesArtist> moviesArtisRepository)
        {
            this.moviesRepository = moviesRepository;
            this.directorsRepository = directorsRepository;
            this.moviesCategoriesRepository = moviesCategoriesRepository;
            this.moviesArtisRepository = moviesArtisRepository;
        }

        public int GetCountSearcingResult(string searchText)
        {
            var moviesCount = this.moviesRepository.AllAsNoTracking()
                               .Where(x => (x.Name.Contains(searchText)
                                   || x.Year.ToString().Contains(searchText)
                                   || x.Artists.Any(x => x.Artist.Name.ToLower().Contains(searchText.ToLower())))
                                   && x.IsDeleted == false)
                               .Count();
            return moviesCount;
        }

        public ICollection<OutputMovieViewModel> SearchMovie(string searchText, int page, int itemPerPage)
        {
            var movies = this.moviesRepository.AllAsNoTracking()
                           .Where(x => (x.Name.Contains(searchText)
                               || x.Year.ToString().Contains(searchText)
                               || x.Artists.Any(x => x.Artist.Name.ToLower().Contains(searchText.ToLower())))
                               && x.IsDeleted == false)
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
            return movies;
        }

        public DetailsMovieViewModel Details(string search, int movieId)
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
                SearchingResult = search,
            };

            details.Director = this.directorsRepository.AllAsNoTracking()
                                              .Where(x => x.Id == movie.DirectorId)
                                              .Select(x => x.Name)
                                              .FirstOrDefault();

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
    }
}

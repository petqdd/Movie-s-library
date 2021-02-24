namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Movies;

    public interface IMoviesService
    {
        Task CreateMovieAsync(InputCreateMovieViewModel model);

        IEnumerable<T> GetAllMovies<T>(int page, int itemPerPage = 15);

        int GetMoviesCount();

        int GetMoviesCountInCollection(string userId);

        int GetMoviesCountInCategory(string category);

        bool IsMovieCollected(int movieId, string userId);

        DetailsMovieViewModel Details(int movieId);

        Task AddFilmToUserCollectionAsync(string userId, int movieId);

        Task RemoveFromCollectionAsync(string userId, int movieId);

        Task DeleteMovieAsync(int movieId);

        InputEditMovieViewModel GetMovieForEdit(int movieId);

        Task EditMovieAsync(int movieId, InputEditMovieViewModel model);

        ICollection<OutputMovieViewModel> GetAllMoviesInMyCollection(string userId, int page, int itemPerPage);

        ICollection<OutputMovieViewModel> GetAllMoviesInCategory(string category, int page, int itemPerPage);

        //IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairsArtists(int movieId);

        ICollection<OutputMovieViewModel> GetTop15MovieImdb();

        //double CalculateTotalUserRating(int movieId);

        // bool IsMovieExisting(string movieName);
    }
}

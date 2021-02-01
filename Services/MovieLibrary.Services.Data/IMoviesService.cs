namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Movies;

    public interface IMoviesService
    {
        Task CreateMovieAsync(InputCreateMovieViewModel model);

        ICollection<OutputMovieViewModel> GetAllMovies();

        double CalculateTotalUserRating(int movieId);

        bool IsMovieExisting(string movieName);

        bool IsMovieCollected(int movieId, string userId);

        DetailsMovieViewModel Details(int movieId);

        Task AddFilmToUserCollectionAsync(string userId, int movieId);

        Task RemoveFromCollectionAsync(string userId, int movieId);

        Task DeleteMovieAsync(int movieId);

        Task EditMovieAsync(int movieId, InputCreateMovieViewModel model);

        InputCreateMovieViewModel GetMovieForEdit(int movieId);

        ICollection<OutputMovieViewModel> GetAllMoviesInMyCollection(string userId);

        ICollection<OutputMovieViewModel> GetAllMoviesInCategory(string category);
    }
}

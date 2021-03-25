namespace MovieLibrary.Services
{
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Movies;

    public interface IImdbScraperService
    {
        Task PopulateDbWithMovies(InputMovieForDbViewModel model);
    }
}

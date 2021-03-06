namespace MovieLibrary.Services
{
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Movies;

    public interface IDatabaseScraperService
    {
        Task PopulateDbWithMovies(InputMovieForDbViewModel model);
    }
}

namespace MovieLibrary.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImdbScraperService
    {
        Task PopulateDbWithMovies();
    }
}

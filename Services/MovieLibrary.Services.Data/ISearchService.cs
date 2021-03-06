namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;

    using MovieLibrary.Web.ViewModels.Movies;

    public interface ISearchService
    {
        ICollection<OutputMovieViewModel> SearchMovie(string searchText, int page, int itemPerPage);

        DetailsMovieViewModel Details(string search, int movieId);

        int GetCountSearcingResult(string searchText);
    }
}

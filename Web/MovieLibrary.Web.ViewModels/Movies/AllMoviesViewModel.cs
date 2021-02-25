namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    public class AllMoviesViewModel : PagingViewModel
    {
        public IEnumerable<OutputMovieViewModel> Movies { get; set; }

        public string SearchingResult { get; set; }
    }
}

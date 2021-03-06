namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    public class AllMoviesInCategoryViewModel : PagingViewModel
    {
        public IEnumerable<OutputMovieViewModel> Movies { get; set; }

        public string Category { get; set; }
    }
}

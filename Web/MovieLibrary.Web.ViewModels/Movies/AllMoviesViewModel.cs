namespace MovieLibrary.Web.Views.ViewModels
{
    using System.Collections.Generic;

    using MovieLibrary.Web.ViewModels;
    using MovieLibrary.Web.ViewModels.Movies;

    public class AllMoviesViewModel : PagingViewModel
    {
        public IEnumerable<OutputMovieViewModel> Movies { get; set; }

        //public string Category { get; set; }
    }
}

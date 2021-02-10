namespace MovieLibrary.Web.Views.ViewModels
{
    using System.Collections.Generic;

    using MovieLibrary.Web.ViewModels.Movies;

    public class AllMoviesViewModel
    {
        public AllMoviesViewModel()
        {
            this.Movies = new HashSet<OutputMovieViewModel>();

        }

        public ICollection<OutputMovieViewModel> Movies { get; set; }

        public string Category { get; set; }
    }
}

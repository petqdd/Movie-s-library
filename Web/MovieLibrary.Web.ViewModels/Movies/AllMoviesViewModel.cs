namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;

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

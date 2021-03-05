namespace MovieLibrary.Web.Views.ViewModels
{
    using System.Collections.Generic;

    using MovieLibrary.Web.ViewModels.Movies;

    public class RandomMoviesViewModel
    {
        public RandomMoviesViewModel()
        {
            this.Movies = new HashSet<OutputViewComponentViewModel>();
        }

        public ICollection<OutputViewComponentViewModel> Movies { get; set; }
    }
}

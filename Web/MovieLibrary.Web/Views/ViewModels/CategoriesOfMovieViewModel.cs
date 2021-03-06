namespace MovieLibrary.Web.Views.ViewModels
{
    using System.Collections.Generic;

    using MovieLibrary.Web.ViewModels.Categories;

    public class CategoriesOfMovieViewModel
    {
        public CategoriesOfMovieViewModel()
        {
            this.CategoriesMovies = new HashSet<OutputCategoriesViewModel>();
        }

        public ICollection<OutputCategoriesViewModel> CategoriesMovies { get; set; }

        public int AllMoviesCount { get; set; }
    }
}

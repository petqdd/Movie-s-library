namespace MovieLibrary.Web.Views.Shared.ViewModels
{
    using System.Collections.Generic;

    using MovieLibrary.Web.ViewModels.Categories;

    public class MovieCategoriesViewModel
    {
        public MovieCategoriesViewModel()
        {
            this.Categories = new HashSet<OutputCategoriesViewModel>();
        }

        public ICollection<OutputCategoriesViewModel> Categories { get; set; }
    }
}

namespace MovieLibrary.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Data;
    using MovieLibrary.Web.ViewModels.Categories;
    using MovieLibrary.Web.Views.ViewModels;

    [ViewComponent(Name = "CategoriesOfMovie")]
    public class CategoriesOfMovieViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public CategoriesOfMovieViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new CategoriesOfMovieViewModel
            {
                CategoriesMovies = this.db.Categories
                               .Select(x => new OutputCategoriesViewModel
                                            {
                                              Name = x.Name,
                                              FilmCount = x.Movies.Count,
                                            })
                               .ToList(),
            };
            return this.View(viewModel);
        }
    }
}

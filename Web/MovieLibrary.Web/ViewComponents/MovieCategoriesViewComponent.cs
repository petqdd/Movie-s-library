namespace MovieLibrary.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Data;
    using MovieLibrary.Web.ViewModels.Categories;
    using MovieLibrary.Web.Views.Shared.ViewModels;

    public class MovieCategoriesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public MovieCategoriesViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke(int movieId)
        {
            var viewModel = new MovieCategoriesViewModel
            {
                Categories = this.db.Movies
                .Where(x => x.Id == movieId)
                                   .Select(x => new OutputCategoriesViewModel
                                   {
                                       Name = x.Artists.Select(y => y.Artist.Name).FirstOrDefault(),
                                   })
                                   .ToList(),
            };
            return this.View(viewModel);
        }
    }
}

namespace MovieLibrary.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Data;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Categories;
    using MovieLibrary.Web.Views.Shared.ViewModels;

    public class MovieCategoriesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly IDeletableEntityRepository<Movie> moviesRepository;

        public MovieCategoriesViewComponent(ApplicationDbContext db, IDeletableEntityRepository<Movie> moviesRepository)
        {
            this.db = db;
            this.moviesRepository = moviesRepository;
        }

        public IViewComponentResult Invoke(int movieId)
        {
            var viewModel = new MovieCategoriesViewModel
            {
                Categories = this.moviesRepository
                                 .AllAsNoTracking()
                                 .Where(x => x.Id == movieId)
                                 .Select(x => new OutputCategoriesViewModel
                                   {
                                       Name = x.Artists.Select(y => y.Artist.Name)
                                                       .FirstOrDefault(),
                                   })
                                 .ToList(),
            };
            return this.View(viewModel);
        }
    }
}

namespace MovieLibrary.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Categories;
    using MovieLibrary.Web.Views.ViewModels;

    [ViewComponent(Name = "CategoriesOfMovie")]
    public class CategoriesOfMovieViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Movie> moviesRepository;

        public CategoriesOfMovieViewComponent(IDeletableEntityRepository<Category> categoriesRepository, IDeletableEntityRepository<Movie> moviesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.moviesRepository = moviesRepository;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new CategoriesOfMovieViewModel
            {
                CategoriesMovies = this.categoriesRepository
                                       .AllAsNoTracking()
                                       .Select(x => new OutputCategoriesViewModel
                                       {
                                           Name = x.Name,
                                           MoviesCount = x.Movies.Count,
                                       })
                                       .ToList(),
                AllMoviesCount = this.moviesRepository
                                     .AllAsNoTracking()
                                     .Count(),
            };
            return this.View(viewModel);
        }
    }
}

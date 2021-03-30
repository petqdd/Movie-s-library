namespace MovieLibrary.Web.ViewComponents
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Data;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Movies;
    using MovieLibrary.Web.Views.ViewModels;

    public class RandomMoviesViewComponents : ViewComponent
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;

        public RandomMoviesViewComponents(IDeletableEntityRepository<Movie> moviesRepository)
        {
            this.moviesRepository = moviesRepository;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new RandomMoviesViewModel
            {
                Movies = this.moviesRepository
                             .AllAsNoTracking()
                             .Select(x => new OutputViewComponentViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Year = x.Year,
                                 PosterPath = x.PosterPath,
                             })
                             .OrderBy(x => Guid.NewGuid())
                             .Take(18)
                             .ToList(),
            };
            return this.View(viewModel);
        }
    }
}

namespace MovieLibrary.Web.ViewComponents
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Data;
    using MovieLibrary.Web.ViewModels.Movies;
    using MovieLibrary.Web.Views.ViewModels;

    public class RandomMoviesViewComponents : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public RandomMoviesViewComponents(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new RandomMoviesViewModel
            {
                Movies = this.db.Movies
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

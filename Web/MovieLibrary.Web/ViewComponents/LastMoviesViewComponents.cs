namespace MovieLibrary.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Data;
    using MovieLibrary.Web.ViewModels.Movies;
    using MovieLibrary.Web.Views.ViewModels;

    public class LastMoviesViewComponents : ViewComponent
    {
        private readonly ApplicationDbContext db;

        public LastMoviesViewComponents(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new LastMoviesViewModel
            {
                Movies = this.db.Movies
                                .Select(x => new OutputViewComponentViewModel
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Year = x.Year,
                                    CreatedDate = x.CreatedDate,
                                })
                                .OrderByDescending(x => x.CreatedDate)
                                .Take(15)
                                .ToList(),
            };
            return this.View(viewModel);
        }
    }
}

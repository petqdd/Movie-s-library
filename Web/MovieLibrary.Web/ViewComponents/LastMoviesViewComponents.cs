namespace MovieLibrary.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Movies;
    using MovieLibrary.Web.Views.ViewModels;

    public class LastMoviesViewComponents : ViewComponent
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;

        public LastMoviesViewComponents(IDeletableEntityRepository<Movie> moviesRepository)
        {
            this.moviesRepository = moviesRepository;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new LastMoviesViewModel
            {
                Movies = this.moviesRepository
                             .AllAsNoTracking()
                             .Select(x => new OutputViewComponentViewModel
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Year = x.Year,
                                 CreatedDate = x.CreatedOn,
                             })
                             .OrderByDescending(x => x.CreatedDate)
                             .Take(15)
                             .ToList(),
            };
            return this.View(viewModel);
        }
    }
}

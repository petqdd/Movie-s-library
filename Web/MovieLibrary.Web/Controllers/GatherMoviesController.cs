namespace MovieLibrary.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Common;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Services;
    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Movies;

    public class GatherMoviesController : BaseController
    {
        private readonly IDatabaseScraperService databaseScraperService;
        private readonly IMoviesService movieService;

        public GatherMoviesController(
            IDatabaseScraperService databaseScraperService,
            IMoviesService movieService)
        {
            this.databaseScraperService = databaseScraperService;
            this.movieService = movieService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet]
        public IActionResult AddDb()
        {
            var viewModel = new InputMovieForDbViewModel();
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddDb(InputMovieForDbViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            else
            {
                var firstCount = this.movieService.GetMoviesCount();
                await this.databaseScraperService.PopulateDbWithMovies(model);
                var lastCount = this.movieService.GetMoviesCount();

                if (firstCount < lastCount)
                {
                    this.TempData["Message"] = "You successful added data for movies!";
                }
                else
                {
                    this.TempData["Message"] = "Format of data was wrong!";
                }

                return this.Redirect("/Movies/All");
            }
        }
    }
}

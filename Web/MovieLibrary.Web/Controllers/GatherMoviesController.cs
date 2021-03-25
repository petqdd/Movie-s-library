namespace MovieLibrary.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Common;
    using MovieLibrary.Services;
    using MovieLibrary.Web.ViewModels.Movies;

    public class GatherMoviesController : BaseController
    {
        private readonly IImdbScraperService imdbScraperService;

        public GatherMoviesController(IImdbScraperService imdbScraperService)
        {
            this.imdbScraperService = imdbScraperService;
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
                await this.imdbScraperService.PopulateDbWithMovies(model);
                this.TempData["Message"] = "You successful added data for movies!";
                return this.Redirect("/Movies/All");
            }
        }
    }
}

namespace MovieLibrary.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MovieLibrary.Services;

    public class GatherMoviesController : BaseController
    {
        private readonly IImdbScraperService imdbScraperService;

        public GatherMoviesController(IImdbScraperService imdbScraperService)
        {
            this.imdbScraperService = imdbScraperService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            await this.imdbScraperService.PopulateDbWithMovies();
            return this.View();
        }
    }
}

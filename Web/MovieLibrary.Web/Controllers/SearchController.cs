namespace MovieLibrary.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Movies;

    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public IActionResult Search(string search)
        {
            var viewModel = new AllMoviesViewModel
            {
                Movies = this.searchService.SearchMovie(search),
            };
            return this.View(viewModel);
        }
    }
}

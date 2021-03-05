namespace MovieLibrary.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Movies;

    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        private readonly IMoviesService moviesService;

        public SearchController(ISearchService searchService, IMoviesService moviesService)
        {
            this.searchService = searchService;
            this.moviesService = moviesService;
        }

        [Authorize]
        public IActionResult Search(string search, int page = 1)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                const int ItemPerPage = 15;

                var viewModel = new AllMoviesViewModel
                {
                    SearchingResult = search,
                    Movies = this.searchService.SearchMovie(search, page, ItemPerPage),
                    Count = this.searchService.GetCountSearcingResult(search),
                    ItemsPerPage = ItemPerPage,
                    PageNumber = page,
                };
                return this.View(viewModel);
            }
        }

        [Authorize]
        public IActionResult Details(string search, int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = this.searchService.Details(search, id);
            viewModel.CollectIsNotAvailable = this.moviesService.IsMovieCollected(viewModel.Id, userId);

            return this.View(viewModel);
        }
    }
}

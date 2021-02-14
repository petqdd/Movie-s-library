namespace MovieLibrary.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Movies;
    using MovieLibrary.Web.Views.ViewModels;

    public class MoviesController : BaseController
    {
        private readonly IMoviesService moviesService;
        private readonly ICategoriesService categoriesService;
        //private readonly IUsersService usersService;

        public MoviesController(IMoviesService moviesService, ICategoriesService categoriesService /*IUsersService usersService*/)
        {
            this.moviesService = moviesService;
            this.categoriesService = categoriesService;
            //this.usersService = usersService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new InputCreateMovieViewModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Add(InputCreateMovieViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(model);
            }
            else
            {
                await this.moviesService.CreateMovieAsync(model);
                return this.RedirectToAction("All");
            }
        }

        [Authorize]
        public IActionResult All(int id = 1)
        {
            const int ItemPerPage = 15;
            var viewModel = new AllMoviesViewModel
            {
                Movies = this.moviesService.GetAllMovies<OutputMovieViewModel>(id, ItemPerPage),
                MoviesCount = this.moviesService.GetCount(),
                ItemsPerPage = ItemPerPage,
                PageNumber = id,
            };
            return this.View(viewModel);
            //var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //foreach (var model in viewModel.Movies)
            //{
            //    model.CollectIsNotAvailable = this.moviesService.IsMovieCollected(model.Id, userId);
            //    //model.UserRating = this.moviesService.CalculateTotalUserRating(model.Id);
            //}
        }

        // [Authorize]
        //public IActionResult Details(int movieId)
        //{
        //    var userId = this.GetUserId();
        //    var viewModel = this.moviesService.Details(movieId);
        //    viewModel.CollectIsNotAvailable = this.moviesService.IsMovieCollected(viewModel.Id, userId);

        //    return this.View(viewModel);
        //}

        //public async Task<IActionResult> AddToCollection(int movieId)
        //{
        //    string userId = this.GetUserId();

        //    await this.moviesService.AddFilmToUserCollectionAsync(userId, movieId);
        //    return this.Redirect("/Movies/All");
        //}

        //public IActionResult Collection()
        //{
        //    string userId = this.GetUserId();
        //    var viewModel = new AllMoviesViewModel
        //    {
        //        Movies = this.moviesService.GetAllMoviesInMyCollection(userId),
        //    };
        //    return this.View(viewModel);
        //}

        //public async Task<IActionResult> RemoveFromCollection(int movieId)
        //{
        //    //var test = this.HttpContext.User.Identity.Name;
        //    var userId = this.GetUserId();
        //    await this.moviesService.RemoveFromCollectionAsync(userId, movieId);
        //    return this.Redirect("/Movies/Collection");
        //}

        //public async Task<IActionResult> Delete(int movieId)
        //{
        //    await this.moviesService.DeleteMovieAsync(movieId);
        //    return this.Redirect("/Movies/All");
        //}

        //[HttpGet]
        //public IActionResult Edit(int movieId)
        //{
        //    var viewModel = this.moviesService.GetMovieForEdit(movieId);
        //    return this.View(viewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(int movieId, InputCreateMovieViewModel model)
        //{
        //    await this.moviesService.EditMovieAsync(movieId, model);
        //    return this.Redirect("/Movies/All");
        //}

        //public IActionResult AllMoviesInCategory(string category)
        //{
        //    var userId = this.GetUserId();
        //    var viewModel = new AllMoviesViewModel
        //    {
        //        Movies = this.moviesService.GetAllMoviesInCategory(category),
        //        Category = category,
        //    };
        //    foreach (var model in viewModel.Movies)
        //    {
        //        model.CollectIsNotAvailable = this.moviesService.IsMovieCollected(model.Id, userId);
        //        //model.UserRating = this.moviesService.CalculateTotalUserRating(model.Id);
        //    }

        //    return this.View(viewModel);
        //}

        //private string GetUserId()
        //{
        //    var userEmail = this.User.FindFirstValue(ClaimTypes.Email);
        //    var userId = this.usersService.GetUserId(userEmail);
        //    return userId;
        //}
    }
}

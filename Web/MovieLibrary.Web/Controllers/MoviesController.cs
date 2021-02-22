namespace MovieLibrary.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MovieLibrary.Common;
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

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new InputCreateMovieViewModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
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
                Count = this.moviesService.GetMoviesCount(),
                ItemsPerPage = ItemPerPage,
                PageNumber = id,
            };
            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = this.moviesService.Details(id);
            viewModel.CollectIsNotAvailable = this.moviesService.IsMovieCollected(viewModel.Id, userId);

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> AddToCollection(int id)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.moviesService.AddFilmToUserCollectionAsync(userId, id);
            return this.RedirectToAction("All");
        }

        [Authorize]
        public IActionResult Collection(int id = 1)
        {
            const int ItemPerPage = 15;
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new AllMoviesViewModel
            {
                Movies = this.moviesService.GetAllMoviesInMyCollection(userId, id, ItemPerPage),
                Count = this.moviesService.GetMoviesCountInCollection(userId),
                ItemsPerPage = ItemPerPage,
                PageNumber = id,
            };
            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            //var test = this.HttpContext.User.Identity.Name;
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.moviesService.RemoveFromCollectionAsync(userId, id);
            return this.RedirectToAction("Collection");
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.moviesService.DeleteMovieAsync(id);
            return this.RedirectToAction("All");
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = this.moviesService.GetMovieForEdit(id);
            return this.View(viewModel);
        }

        //[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        //[HttpPost]
        //public async Task<IActionResult> Edit(int movieId, InputCreateMovieViewModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View();
        //    }
        //    else
        //    {
        //        await this.moviesService.EditMovieAsync(movieId, model);
        //        return this.RedirectToAction("All");
        //    }
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
    }
}

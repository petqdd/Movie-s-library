namespace MovieLibrary.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MovieLibrary.Common;
    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Movies;

    public class MoviesController : BaseController
    {
        private readonly IMoviesService moviesService;
        private readonly ICategoriesService categoriesService;

        public MoviesController(IMoviesService moviesService, ICategoriesService categoriesService /*IUsersService usersService*/)
        {
            this.moviesService = moviesService;
            this.categoriesService = categoriesService;
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
                this.TempData["Message"] = "Movie added successfully!";
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
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            //viewModel.ArtistsItems = this.moviesService.GetAllAsKeyValuePairsArtists(id);
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, InputEditMovieViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = model;
                viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(viewModel);
            }
            else
            {
                if (model.Categories.Count == 0)
                {
                    var viewModel = model;
                    viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                    return this.View(viewModel);
                }

                await this.moviesService.EditMovieAsync(id, model);
                this.TempData["Message"] = "Movie edited successfully!";
                return this.Redirect($"/Movies/Details/{id}");
            }
        }

        [Authorize]
        public IActionResult AllMoviesInCategory(string category, int id = 1)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            const int ItemPerPage = 15;
            var viewModel = new AllMoviesInCategoryViewModel
            {
                Movies = this.moviesService.GetAllMoviesInCategory(category, id, ItemPerPage),
                Category = category,
                Count = this.moviesService.GetMoviesCountInCategory(category),
                ItemsPerPage = ItemPerPage,
                PageNumber = id,
            };
            foreach (var model in viewModel.Movies)
            {
                model.CollectIsNotAvailable = this.moviesService.IsMovieCollected(model.Id, userId);
                //model.UserRating = this.moviesService.CalculateTotalUserRating(model.Id);
            }

            return this.View(viewModel);
        }

        public IActionResult Top15Movies()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new AllMoviesViewModel
            {
                Movies = this.moviesService.GetTop15MovieImdb(),
            };
            foreach (var movie in viewModel.Movies)
            {
                movie.CollectIsNotAvailable = this.moviesService.IsMovieCollected(movie.Id, userId);
            }

            return this.View(viewModel);
        }
    }
}

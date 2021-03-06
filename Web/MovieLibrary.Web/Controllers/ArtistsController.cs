namespace MovieLibrary.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MovieLibrary.Common;
    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Artists;

    public class ArtistsController : BaseController
    {
        private readonly IArtistService artistService;

        public ArtistsController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Add()
        {
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Add(InputCreateArtistViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                if (this.artistService.CheckForExistingArtist(model.Name))
                {
                    await this.artistService.EditArtistAsync(model);
                }
                else
                {
                    await this.artistService.CreateArtistAsync(model);
                }

                this.TempData["Message"] = "Artist added successfully!";
                return this.RedirectToAction("All");
            }
        }

        [Authorize]
        public IActionResult All(int id = 1)
        {
            const int ItemPerPage = 15;
            var viewModel = new AllArtistsViewModel
            {
                Artists = this.artistService.GetAllArtists(id, ItemPerPage),
                ItemsPerPage = ItemPerPage,
                PageNumber = id,
                Count = this.artistService.GetArtistsCount(),
            };
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpGet]
        public IActionResult Edit(string artist)
        {
            var viewModel = this.artistService.GetArtistForEdit(artist);
            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Edit(string artist, InputCreateArtistViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                await this.artistService.EditArtistAsync(artist, model);
                this.TempData["Message"] = "Artist edited successfully!";
                return this.RedirectToAction("All");
            }
        }
    }
}

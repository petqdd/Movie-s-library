namespace MovieLibrary.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Artists;

    public class ArtistsController : BaseController
    {
        private readonly IArtistService artistService;

        public ArtistsController(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        public IActionResult Add()
        {
            return this.View();
        }

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

                return this.Redirect("/Artists/All");
            }
        }

        public IActionResult All()
        {
            var viewModel = new AllArtistsViewModel
            {
                Artists = this.artistService.GetAllArtists(),
            };
            return this.View(viewModel);
        }

        public IActionResult Detail(string artist, int movieId)
        {
            var viewModel = this.artistService.GetArtistInfo(artist, movieId);
            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(string artist)
        {
            var viewModel = this.artistService.GetArtistForEdit(artist);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string artist, InputCreateArtistViewModel model)
        {
            await this.artistService.EditArtistAsync(artist, model);
            return this.Redirect("/Artists/All");
        }
    }
}

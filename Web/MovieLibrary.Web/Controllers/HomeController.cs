namespace MovieLibrary.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Common;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Services;
    using MovieLibrary.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IImdbScraperService imdbScraperService;

        public HomeController(SignInManager<ApplicationUser> signInManager, IImdbScraperService imdbScraperService)
        {
            this.signInManager = signInManager;
            this.imdbScraperService = imdbScraperService;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            if (this.signInManager.IsSignedIn(this.User))
            {
                return this.Redirect("/Movies/All");
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Chat()
        {
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult>AddDb()
        {
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> ScraperDb()
        {
            await this.imdbScraperService.PopulateDbWithMovies();
            this.TempData["Message"] = "You successful added data for movies!";
            return this.RedirectToPage("/Movies/All");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [HttpGet("robots.txt")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
        public IActionResult RobotsTxt() =>
            this.Content("User-agent: *" + Environment.NewLine + "Disallow:");
    }
}

namespace MovieLibrary.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Common;
    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Users;

    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsersController(IUsersService usersService, IWebHostEnvironment webHostEnvironment)
        {
            this.usersService = usersService;
            this.webHostEnvironment = webHostEnvironment;
        }

        //[Authorize(Roles = GlobalConstants.UserRoleName)]
        public IActionResult Profile()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Profile(InputUsersViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            else
            {
                try
                {
                    model.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var imagePath = this.webHostEnvironment.WebRootPath;
                    await this.usersService.AddUserPhoto(model, imagePath);
                    this.TempData["Message"] = "Profile picture added successfully!";
                    return this.Redirect("/Movies/All");
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                    return this.View();
                }
            }
        }
    }
}

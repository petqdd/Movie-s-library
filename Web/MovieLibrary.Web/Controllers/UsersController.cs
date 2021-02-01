namespace MovieLibrary.Web.Controllers
{
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels.Users;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsersController(IUsersService usersService, IWebHostEnvironment webHostEnvironment)
        {
            this.usersService = usersService;
            this.webHostEnvironment = webHostEnvironment;
        }

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
                var userEmail = this.User.FindFirstValue(ClaimTypes.Email);
                //var userEmail = this.User.Identity.Name;
                var userId = this.usersService.GetUserId(userEmail);
                model.UserId = userId;
                await this.usersService.AddUserPhoto(model);
                return this.Redirect("/");
            }

        }

        //[HttpPost]
        //public async Task<IActionResult> Profile(InputUsersViewModel model)
        //{

        //    var userEmail = this.User.Identity.Name;
        //    var userId = this.usersService.GetUserId(userEmail);
        //    var filepath = this.webHostEnvironment.WebRootPath + $"user{model.UserId}.png";
        //    using (var stream = new FileStream(filepath, FileMode.Create))
        //    {
        //        await model.Image.CopyToAsync(stream);
        //    }
        //    return this.Redirect("/");
        //}
    }
}

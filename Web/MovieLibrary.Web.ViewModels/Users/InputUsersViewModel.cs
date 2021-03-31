namespace MovieLibrary.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class InputUsersViewModel
    {
        public string UserId { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}

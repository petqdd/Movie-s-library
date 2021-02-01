namespace MovieLibrary.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    public class InputUsersViewModel
    {
        public string UserId { get; set; }

        [Required]
        [Url]
        public string PhotoPath { get; set; }

        //public IFormFile Image { get; set; }
    }
}

namespace MovieLibrary.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class InputCreateCategoryViewModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(25)]

        public string Name { get; set; }
    }
}

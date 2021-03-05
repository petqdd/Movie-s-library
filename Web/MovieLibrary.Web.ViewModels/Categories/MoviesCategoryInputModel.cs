namespace MovieLibrary.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class MoviesCategoryInputModel
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(30)]
        public string CategoryName { get; set; }
    }
}

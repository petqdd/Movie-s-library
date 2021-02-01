namespace MovieLibrary.Web.ViewModels.Artists
{
    using System.ComponentModel.DataAnnotations;

    public class MovieArtistInputModel
    {
        [Required]
        [StringLength(50, MinimumLength =2)]
        public string Name { get; set; }
    }
}

namespace MovieLibrary.Web.ViewModels.Artists
{
    using System.ComponentModel.DataAnnotations;

    public class InputCreateArtistViewModel
    {
        [Required]
        [MaxLength(30)]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        [Url]
        public string BiographyUrl { get; set; }

        [Required]
        [Url]
        public string PhotoUrl { get; set; }
    }
}

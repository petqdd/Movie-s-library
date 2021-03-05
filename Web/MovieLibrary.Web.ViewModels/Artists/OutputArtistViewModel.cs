namespace MovieLibrary.Web.ViewModels.Artists
{
    using System.ComponentModel.DataAnnotations;

    public class OutputArtistViewModel
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string BiographyUrl { get; set; }

        public string PhotoUrl { get; set; }

        public int MovieId { get; set; }
    }
}

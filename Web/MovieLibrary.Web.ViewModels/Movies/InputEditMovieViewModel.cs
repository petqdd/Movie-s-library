namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Web.ValidationsAttribute;

    public class InputEditMovieViewModel
    {
        public InputEditMovieViewModel()
        {
            this.Artists = new HashSet<string>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [CurrentYearMaxValue(1800)]
        public int Year { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }


        [Range(1, 10 * 60)]
        public int Runtime { get; set; }

        [Required]
        [MaxLength(200)]
        [Url]
        public string PosterPath { get; set; }

        [Required]
        [MaxLength(200)]
        [Url]
        public string TrailerUrl { get; set; }

        [Required]
        public string Storyline { get; set; }

        [Range(1, 10.0)]
        [Display(Name = "Imdb Rating")]
        public double ImdbRating { get; set; }

        public ICollection<string> Artists { get; set; }
    }
}
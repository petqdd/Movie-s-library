namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Web.ValidationsAttribute;
    using MovieLibrary.Web.ViewModels.Artists;
    using MovieLibrary.Web.ViewModels.Categories;

    public class InputCreateMovieViewModel
    {
        public InputCreateMovieViewModel()
        {
            this.Artists = new HashSet<MovieArtistInputModel>();
            this.Categories = new HashSet<MoviesCategoryInputModel>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [CurrentYearMaxValue(1800)]
        public int Year { get; set; }

        //[Display(Name="Category")]
        //public int CategoryId { get; set; }

        [Range(1, 10 * 60)]
        public int Runtime { get; set; }

        [Required]
        [MaxLength(200)]
        [Url]
        public string PosterPath { get; set; }

        [Required]
        [MaxLength(200)]
        [Url]
        public string SecondPosterPath { get; set; }

        [Required]
        [MaxLength(200)]
        [Url]
        public string TrailerUrl { get; set; }

        [Required]
        public string Storyline { get; set; }

        [Required]
        [MaxLength(100)]
        public string Director { get; set; }

        [Range(1, 10.0)]
        [Display(Name= "Imdb Rating")]
        public double ImdbRating { get; set; }

        public ICollection<MovieArtistInputModel> Artists { get; set; }

        public ICollection<MoviesCategoryInputModel> Categories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }
    }
}

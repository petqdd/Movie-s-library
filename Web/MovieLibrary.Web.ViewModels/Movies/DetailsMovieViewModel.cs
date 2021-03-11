namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    using MovieLibrary.Data.Models;

    public class DetailsMovieViewModel
    {
        public DetailsMovieViewModel()
        {
            this.Artists = new HashSet<Artist>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public string PosterUrl { get; set; }

        public string Storyline { get; set; }

        public string Director { get; set; }

        public int Runtime { get; set; }

        public string TrailerUrl { get; set; }

        public string SecondPosterUrl { get; set; }

        public double ImdbRating { get; set; }

        public double UserRating { get; set; }

        public string Categories { get; set; }

        public bool CollectIsNotAvailable { get; set; }

        public double AverageRating { get; set; }

        public ICollection<Artist> Artists { get; set; }

        public string SearchingResult { get; set; }
    }
}

namespace MovieLibrary.Services.Models
{
    using System.Collections.Generic;

    public class MovieDto
    {
        public MovieDto()
        {
            this.Artists = new List<string>();
            this.CategoryName = new List<string>();
        }

        public string MovieName { get; set; }

        public int Year { get; set; }

        public int Runtime { get; set; }

        public string PosterPath { get; set; }

        public string SecondPosterPath { get; set; }

        public string TrailerUrl { get; set; }

        public string Storyline { get; set; }

        public double ImdbRating { get; set; }

        public double UserRating { get; set; }

        public string Director { get; set; }

        public List<string> CategoryName { get; set; }

        public List<string> Artists { get; set; }
    }
}

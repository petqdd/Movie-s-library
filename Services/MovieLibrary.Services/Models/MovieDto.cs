namespace MovieLibrary.Services.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class MovieDto
    {
        public MovieDto()
        {
            this.Artists = new List<ArtistDto>();
            this.CategoryName = new List<Genre>();
        }

        [JsonProperty("original_title")]
        public string MovieName { get; set; }

        [JsonProperty("release_date")]
        public string Year { get; set; }

        [JsonProperty("runtime")]
        public int Runtime { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("backdrop_path")]
        public string SecondPosterPath { get; set; }

        public string TrailerUrl { get; set; }

        [JsonProperty("overview")]
        public string Storyline { get; set; }

        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }

        public double ImdbRating { get; set; }

        public double UserRating { get; set; }

        [JsonProperty("xxx")]
        public string Director { get; set; }

        [JsonProperty("genres")]
        public List<Genre> CategoryName { get; set; }

        [JsonProperty("xxxxx")]
        public List<ArtistDto> Artists { get; set; }
    }
}

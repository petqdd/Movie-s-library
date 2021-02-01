namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    using MovieLibrary.Data.Models;

    public class DetailsMovieViewModel
    {
        public int Id { get; set; }

        public string PosterUrl { get; set; }

        public string Storyline { get; set; }

        public ICollection<Artist> Artists { get; set; }
    }
}

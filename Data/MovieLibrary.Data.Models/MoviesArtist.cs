namespace MovieLibrary.Data.Models
{
    using MovieLibrary.Data.Common.Models;


    public class MoviesArtist : BaseModel<int>
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int ArtistId { get; set; }

        public Artist Artist { get; set; }
    }
}

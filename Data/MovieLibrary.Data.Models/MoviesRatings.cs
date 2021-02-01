namespace MovieLibrary.Data.Models
{
    using MovieLibrary.Data.Common.Models;


    public class MoviesRatings : BaseModel<int>
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int RatingId { get; set; }

        public Rating Rating { get; set; }
    }
}

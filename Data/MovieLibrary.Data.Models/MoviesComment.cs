namespace MovieLibrary.Data.Models
{
    using MovieLibrary.Data.Common.Models;

    public class MoviesComment : BaseModel<int>
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int CommentId { get; set; }

        public Comment Comments { get; set; }
    }
}

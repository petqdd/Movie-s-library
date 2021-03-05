namespace MovieLibrary.Data.Models
{
    using MovieLibrary.Data.Common.Models;

    public class MoviesCategory : BaseModel<int>
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}

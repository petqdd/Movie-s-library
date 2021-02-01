namespace MovieLibrary.Data.Models
{
    using MovieLibrary.Data.Common.Models;

    public class UsersMovie : BaseModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}

namespace MovieLibrary.Data.Models
{
    using MovieLibrary.Data.Common.Models;

    public class UsersRatings : BaseModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int RatingId { get; set; }

        public Rating Rating { get; set; }
    }
}

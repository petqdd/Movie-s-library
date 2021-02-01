namespace MovieLibrary.Data.Models
{
    using MovieLibrary.Data.Common.Models;

    public class UsersComment : BaseModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CommentId { get; set; }

        public Comment Comment { get; set; }
    }
}

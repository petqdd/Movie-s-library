namespace MovieLibrary.Web.ViewModels.Ratings
{
    using System.ComponentModel.DataAnnotations;

    public class InputCreateRatingViewModel
    {
        public int MovieId { get; set; }

        public string UserId { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }
    }
}

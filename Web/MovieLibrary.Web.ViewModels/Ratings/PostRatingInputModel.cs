namespace MovieLibrary.Web.ViewModels.Ratings
{
    using System.ComponentModel.DataAnnotations;

    public class PostRatingInputModel
    {
        public int MovieId { get; set; }

        [Range(1, 10)]
        public int Value { get; set; }
    }
}

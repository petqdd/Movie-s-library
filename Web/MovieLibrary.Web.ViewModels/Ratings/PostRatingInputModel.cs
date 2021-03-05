using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.Web.ViewModels.Ratings
{
    public class PostRatingInputModel
    {
        public int MovieId { get; set; }

        [Range(1, 10)]
        public int Value { get; set; }
    }
}

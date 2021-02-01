namespace MovieLibrary.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class InputCommentViewModel
    {
        [Required]
        [MaxLength(250)]
        public string Content { get; set; }

        public int MovieId { get; set; }
    }
}

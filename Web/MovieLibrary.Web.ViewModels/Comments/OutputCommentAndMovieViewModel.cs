namespace MovieLibrary.Web.ViewModels.Comments
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OutputCommentAndMovieViewModel
    {
        public int MovieId { get; set; }

        public string MovieName { get; set; }

        [Required]
        [MaxLength(250)]
        public string CommentContent { get; set; }

        public int CommentId { get; set; }

        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserPhoto { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

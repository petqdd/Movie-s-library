namespace MovieLibrary.Web.ViewModels.Comments
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OutputCommentViewModel
    {
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

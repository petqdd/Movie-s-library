namespace MovieLibrary.Web.ViewModels.Comments
{
    using System.Collections.Generic;

    public class CommentsViewModel
    {
        public int MovieId { get; set; }

        public IEnumerable<OutputAllCommentsViewModel> AllComments { get; set; }
    }
}

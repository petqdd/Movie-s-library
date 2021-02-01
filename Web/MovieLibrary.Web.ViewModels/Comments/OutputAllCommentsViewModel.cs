namespace MovieLibrary.Web.ViewModels.Comments
{
    using System.Collections.Generic;

    public class OutputAllCommentsViewModel
    {
        public int MovieId { get; set; }

        public IEnumerable<OutputCommentViewModel> Comments { get; set; }
    }
}

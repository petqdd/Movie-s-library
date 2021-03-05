namespace MovieLibrary.Web.ViewModels.Comments
{
    using System.Collections.Generic;

    public class OutputAllComentsAndMoviesViewModel : PagingViewModel
    {
        public IEnumerable<OutputCommentAndMovieViewModel> Comments { get; set; }
    }
}

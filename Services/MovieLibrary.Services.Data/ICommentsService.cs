namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        Task CreateCommentAsync(InputCommentViewModel model, string userId);

        Task DeleteCommentAsync(int commentId);

        OutputAllCommentsViewModel GetAllComment(int movieId, int page, int itemPerPage);

        OutputAllComentsAndMoviesViewModel GetAllComment(int page, int itemPerPage);

        int GetCommentsCount(int id);

        int GetAllCommentsCount();
    }
}

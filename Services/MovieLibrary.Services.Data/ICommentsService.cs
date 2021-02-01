namespace MovieLibrary.Web.Services
{
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Comments;

    public interface ICommentsService
    {
        Task CreateCommentAsync(InputCommentViewModel model, string userId);

        Task DeleteCommentAsync(int commentId);

        OutputAllCommentsViewModel GetAllComment(int movieId);

        OutputAllComentsAndMoviesViewModel GetAllComment();
    }
}

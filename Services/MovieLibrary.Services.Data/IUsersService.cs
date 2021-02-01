namespace MovieLibrary.Web.Services
{
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Users;

    public interface IUsersService
    {
        string GetUserId(string email);

        Task AddUserPhoto(InputUsersViewModel model);
    }
}

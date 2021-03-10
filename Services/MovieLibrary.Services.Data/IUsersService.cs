namespace MovieLibrary.Web.Services
{
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Users;

    public interface IUsersService
    {
        Task AddUserPhoto(InputUsersViewModel model, string imagePath);
    }
}

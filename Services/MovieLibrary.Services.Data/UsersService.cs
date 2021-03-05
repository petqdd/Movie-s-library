namespace MovieLibrary.Web.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        //public string GetUserId(string email)
        //{
        //    var userId = this.usersRepository
        //                     .AllAsNoTracking()
        //                     .Where(x => x.Email == email)
        //                     .Select(x => x.Id)
        //                     .FirstOrDefault();
        //    return userId;
        //}

        //public async Task AddUserPhoto(InputUsersViewModel model)
        //{
        //    var user = this.usersRepository
        //                   .AllAsNoTracking()
        //                   .Where(x => x.Id == model.UserId)
        //                   .FirstOrDefault();

        //    //user.Photo = model.PhotoPath;
        //    this.usersRepository.Update(user);
        //    await this.usersRepository.SaveChangesAsync();
        //}
    }
}

namespace MovieLibrary.Web.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Photo> photosRepository;
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<Photo> photosRepository)
        {
            this.usersRepository = usersRepository;
            this.photosRepository = photosRepository;
        }

        public async Task AddUserPhoto(InputUsersViewModel model, string imagePath)
        {
            var user = this.usersRepository
                           .AllAsNoTracking()
                           .Where(x => x.Id == model.UserId)
                           .FirstOrDefault();

            Directory.CreateDirectory($"{imagePath}/images/users/");

            var extension = Path.GetExtension(model.Photo.FileName).TrimStart('.');
            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new Exception($"Invalid image extension {extension}");
            }

            var existingPhoto = this.photosRepository.AllAsNoTracking()
                                                     .Where(x => x.UserId == user.Id)
                                                     .FirstOrDefault();
            if (existingPhoto != null)
            {
                this.photosRepository.Delete(existingPhoto);
                await this.photosRepository.SaveChangesAsync();
            }

            var photo = new Photo
            {
                UserId = user.Id,
                Extension = extension,
            };

            var physicalPath = $"{imagePath}/images/users/{photo.Id}.{extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await model.Photo.CopyToAsync(fileStream);

            photo.RemotePhotoUrl = $"/images/users/{photo.Id}.{extension}";
            await this.photosRepository.AddAsync(photo);
            await this.photosRepository.SaveChangesAsync();
        }
    }
}

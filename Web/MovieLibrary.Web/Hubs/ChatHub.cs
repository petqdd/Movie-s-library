namespace MovieLibrary.Web.Hubs
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Chat;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IRepository<Photo> photosRepository;

        public ChatHub(IRepository<Photo> photosRepository)
        {
            this.photosRepository = photosRepository;
        }

        public async Task Send(string message)
        {
            var currentUser = this.Context.User.Identity.Name;
            var userId = this.Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var photo = this.photosRepository.AllAsNoTracking()
                                           .Where(x => x.Id == userId)
                                           .Select(x => x.RemotePhotoUrl)
                                           .FirstOrDefault();
            if (photo == null)
            {
                photo = "https://s3.amazonaws.com/37assets/svn/765-default-avatar.png";
            }

            await this.Clients.All.SendAsync(
                "NewMessage",
                new Message { User = currentUser, Text = message, Photo = photo });
        }
    }
}

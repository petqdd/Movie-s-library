namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Artists;

    public interface IArtistService
    {
        Task CreateArtistAsync(InputCreateArtistViewModel model);

        IEnumerable<InputCreateArtistViewModel> GetAllArtists(int page, int itemPerPage);

        bool CheckForExistingArtist(string artist);

        Task EditArtistAsync(InputCreateArtistViewModel model);

        int GetArtistsCount();

        InputCreateArtistViewModel GetArtistForEdit(string artist);

        Task EditArtistAsync(string artist, InputCreateArtistViewModel model);
    }
}

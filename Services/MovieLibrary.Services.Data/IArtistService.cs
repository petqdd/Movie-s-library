namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Web.ViewModels.Artists;

    public interface IArtistService
    {
        Task CreateArtistAsync(InputCreateArtistViewModel model);

        IEnumerable<InputCreateArtistViewModel> GetAllArtists();

        OutputArtistViewModel GetArtistInfo(string artist, int movieId);

        bool CheckForExistingArtist(string artist);

        Task EditArtistAsync(InputCreateArtistViewModel model);

        Task EditArtistAsync(string artist, InputCreateArtistViewModel model);

        InputCreateArtistViewModel GetArtistForEdit(string artist);
    }
}

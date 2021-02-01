namespace MovieLibrary.Web.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Web.ViewModels.Artists;

    public class ArtistService : IArtistService
    {
        private readonly IDeletableEntityRepository<Artist> artistsRepository;

        public ArtistService(IDeletableEntityRepository<Artist> artistsRepository)
        {
            this.artistsRepository = artistsRepository;
        }

        public bool CheckForExistingArtist(string artist)
        {
            return this.artistsRepository
                       .AllAsNoTracking()
                       .Any(x => x.Name == artist);
        }

        public async Task CreateArtistAsync(InputCreateArtistViewModel model)
        {
            var artist = new Artist
            {
                Name = model.Name,
                BiographyUrl = model.BiographyUrl,
                PhotoUrl = model.PhotoUrl,
            };

            if (this.artistsRepository
                    .AllAsNoTracking()
                    .Any(x => x.Name == model.Name))
            {
                return;
            }

            await this.artistsRepository.AddAsync(artist);
            await this.artistsRepository.SaveChangesAsync();
        }

        public async Task EditArtistAsync(InputCreateArtistViewModel model)
        {
            var artist = this.artistsRepository
                              .AllAsNoTracking()
                              .FirstOrDefault(x => x.Name == model.Name);
            artist.PhotoUrl = model.PhotoUrl;
            artist.BiographyUrl = model.BiographyUrl;
            this.artistsRepository.Update(artist);
            await this.artistsRepository.SaveChangesAsync();
        }

        public IEnumerable<InputCreateArtistViewModel> GetAllArtists()
        {
            var artists = this.artistsRepository
                .AllAsNoTracking()
                .Where(x => x.Name != null && x.BiographyUrl != null && x.PhotoUrl != null)
                .Select(x => new InputCreateArtistViewModel
                {
                    Name = x.Name,
                    BiographyUrl = x.BiographyUrl,
                    PhotoUrl = x.PhotoUrl,
                }).ToList();
            return artists;
        }

        public OutputArtistViewModel GetArtistInfo(string artist, int movieId)
        {
            var detail = this.artistsRepository
                    .AllAsNoTracking()
                    .Where(x => x.Name == artist)
                    .Select(x => new OutputArtistViewModel
                    {
                        Name = x.Name,
                        BiographyUrl = x.BiographyUrl,
                        PhotoUrl = x.PhotoUrl,
                        MovieId = movieId,
                    })
                    .FirstOrDefault();
            return detail;
        }

        public InputCreateArtistViewModel GetArtistForEdit(string artist)
        {
            return this.artistsRepository
                       .AllAsNoTracking()
                       .Where(x => x.Name == artist)
                       .Select(x => new InputCreateArtistViewModel
                       {
                           Name = x.Name,
                           BiographyUrl = x.BiographyUrl,
                           PhotoUrl = x.PhotoUrl,
                       })
                       .FirstOrDefault();
        }

        public async Task EditArtistAsync(string artist, InputCreateArtistViewModel model)
        {
            var currentArtist = this.artistsRepository
                                    .All()
                                    .Where(x => x.Name == artist)
                                    .FirstOrDefault();
            currentArtist.Name = model.Name;
            currentArtist.BiographyUrl = model.BiographyUrl;
            currentArtist.PhotoUrl = model.PhotoUrl;
            this.artistsRepository.Update(currentArtist);
            await this.artistsRepository.SaveChangesAsync();
        }
    }
}

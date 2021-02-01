namespace MovieLibrary.Web.ViewModels.Artists
{
    using System.Collections.Generic;

    public class AllArtistsViewModel
    {
        public IEnumerable<InputCreateArtistViewModel> Artists { get; set; }
    }
}

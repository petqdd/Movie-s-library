namespace MovieLibrary.Web.ViewModels.Artists
{
    using System.Collections.Generic;

    public class AllArtistsViewModel : PagingViewModel
    {
        public IEnumerable<InputCreateArtistViewModel> Artists { get; set; }

    }
}

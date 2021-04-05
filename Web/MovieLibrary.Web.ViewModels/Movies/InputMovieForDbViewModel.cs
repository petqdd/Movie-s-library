namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;

    public class InputMovieForDbViewModel
    {
        public InputMovieForDbViewModel()
        {
            this.MovieIds = new HashSet<int>();
        }

        public ICollection<int> MovieIds { get; set; }

        //public int StartId { get; set; }

        //public int EndId { get; set; }
    }
}

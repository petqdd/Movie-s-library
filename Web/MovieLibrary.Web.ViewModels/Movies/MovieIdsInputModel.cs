namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.ComponentModel.DataAnnotations;

    public class MovieIdsInputModel
    {
        [Range(0, 100000000)]
        public int Id { get; set; }
    }
}
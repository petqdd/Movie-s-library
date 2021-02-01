//namespace MovieLibrary.Web.Services
//{
//    using System;

//    using System.Collections.Generic;
//    using System.Linq;

//    using MovieLibrary.Data;
//    using MovieLibrary.Web.ViewModels.Movies;

//    public class SearchService : ISearchService
//    {
//        private readonly ApplicationDbContext db;

//        public SearchService(ApplicationDbContext db)
//        {
//            this.db = db;
//        }

//        public ICollection<OutputMovieViewModel> SearchMovie(string searchText)
//        {
//            var movies = this.db.Movies
//                           .Where(x => (x.Name.Contains(searchText)
//                               || x.Year.ToString().Contains(searchText)
//                               || x.Artists.Any(x => x.Artist.Name.ToLower().Contains(searchText.ToLower())))
//                               && x.IsDeleted == false)
//                           .Select(x => new OutputMovieViewModel
//                           {
//                               Id = x.Id,
//                               Name = x.Name,
//                               Year = x.Year,
//                               Runtime = x.Runtime,
//                               ImdbRating = x.ImdbRating,
//                               TrailerUrl = x.TrailerUrl,
//                               PosterPath = x.PosterPath,
//                               Storyline = x.Storyline,
//                               Category = x.Category.Name,
//                           })
//                           .ToList();
//            return movies;
//        }
//    }
//}

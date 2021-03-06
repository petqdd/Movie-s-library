//namespace MovieLibrary.Web.ViewComponents
//{
//    using System.Linq;

//    using Microsoft.AspNetCore.Mvc;

//    using MovieLibrary.Data;
//    using MovieLibrary.Web.ViewModels.Movies;
//    using MovieLibrary.Web.Views.ViewModels;

//    public class TopRatedMoviesViewComponents : ViewComponent
//    {
//        private readonly ApplicationDbContext db;

//        public TopRatedMoviesViewComponents(ApplicationDbContext db)
//        {
//            this.db = db;
//        }

//        public IViewComponentResult Invoke()
//        {
//            var viewModel = new TopRatedMoviesViewModel
//            {
//                Movies = this.db.Movies
//                                .Select(x => new OutputViewComponentViewModel
//                                {
//                                    Id = x.Id,
//                                    Name = x.Name,
//                                    Year = x.Year,
//                                    PosterPath = x.PosterPath,
//                                    Rating = x.ImdbRating,
//                                })
//                                .OrderByDescending(x => x.Rating)
//                                .Take(5)
//                                .ToList(),
//            };
//            return this.View(viewModel);
//        }
//    }
//}
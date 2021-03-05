//namespace MovieLibrary.Web.ViewComponents
//{
//    using System.Linq;

//    using Microsoft.AspNetCore.Mvc;

//    using MovieLibrary.Data;
//    using MovieLibrary.Web.ViewModels.Movies;
//    using MovieLibrary.Web.Views.ViewModels;

//    public class NewestMoviesViewComponents : ViewComponent
//    {
//        private readonly ApplicationDbContext db;

//        public NewestMoviesViewComponents(ApplicationDbContext db)
//        {
//            this.db = db;
//        }

//        public IViewComponentResult Invoke()
//        {
//            var viewModel = new NewestMoviesViewModel
//            {
//                Movies = this.db.Movies
//                                .Select(x => new OutputViewComponentViewModel
//                                {
//                                    Id = x.Id,
//                                    Name = x.Name,
//                                    Year = x.Year,
//                                    PosterPath = x.PosterPath,
//                                })
//                                .OrderByDescending(x => x.Year)
//                                .Take(5)
//                                .ToList(),
//            };
//            return this.View(viewModel);
//        }
//    }
//}

//namespace MovieLibrary.Web.ViewComponents
//{
//    using Microsoft.AspNetCore.Mvc;
//    using MovieLibrary.Web.Services;
//    using MovieLibrary.Web.Views.ViewModels;
//    using System.Security.Claims;

//    public class AllMoviesViewComponent : ViewComponent
//    {
//        private readonly IMoviesService moviesService;

//        public AllMoviesViewComponent(IMoviesService moviesService)
//        {
//            this.moviesService = moviesService;
//        }

//        public IViewComponentResult Invoke()
//        {
//            var viewModel = new AllMoviesViewModel
//            {
//                Movies = this.moviesService.GetAllMovies(),
//            };
//            return this.View(viewModel);
//        }
//    }
//}

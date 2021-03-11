namespace MovieLibrary.Web.ViewModels.Movies
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using MovieLibrary.Data.Models;
    using MovieLibrary.Services.Mapping;

    public class OutputMovieViewModel : IMapFrom<Movie>, IHaveCustomMappings
    {
        public OutputMovieViewModel()
        {
            this.Categories = new HashSet<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public string PosterPath { get; set; }

        public bool CollectIsNotAvailable { get; set; }

        public ICollection<string> Categories { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, OutputMovieViewModel>()
                .ForMember(x => x.Categories, opt =>
                   opt.MapFrom(x => x.Categories.Select(x => x.Category.Name).ToList()));
        }
    }
}

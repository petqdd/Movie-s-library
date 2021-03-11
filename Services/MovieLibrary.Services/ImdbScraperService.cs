namespace MovieLibrary.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using AngleSharp;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Services.Models;

    public class ImdbScraperService : IImdbScraperService
    {
        private readonly IConfiguration config;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Artist> artistsRepository;
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<Director> directorsRepository;
        private readonly IRepository<MoviesArtist> moviesArtistsRepository;
        private readonly IRepository<MoviesCategory> moviesCategoriesRepository;
        private IBrowsingContext context;

        public ImdbScraperService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Artist> artistsRepository,
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<Director> directorsRepository,
            IRepository<MoviesArtist> moviesArtistsRepository,
            IRepository<MoviesCategory> moviesCategoriesRepository)
        {
            this.config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(this.config);
            this.categoriesRepository = categoriesRepository;
            this.artistsRepository = artistsRepository;
            this.moviesRepository = moviesRepository;
            this.directorsRepository = directorsRepository;
            this.moviesArtistsRepository = moviesArtistsRepository;
            this.moviesCategoriesRepository = moviesCategoriesRepository;
        }

        public async Task PopulateDbWithMovies()
        {
            var concurentBag = new ConcurrentBag<MovieDto>();
            //Parallel.For(0111161, movieCount, (i) =>
            //{
            //    try
            //    {
            //        var movie = this.GetMovieInfo(i);
            //        concurentBag.Add(movie);
            //    }
            //    catch { }
            //});
            var numbers = new List<int>();
            numbers.Add(114709);
            numbers.Add(86879);
            numbers.Add(361748);
            numbers.Add(86190);
            numbers.Add(119217);
            numbers.Add(105236);
            numbers.Add(62622);
            numbers.Add(180093);
            numbers.Add(52357);
            numbers.Add(338013);
            numbers.Add(33467);
            numbers.Add(74352);
            numbers.Add(93058);
            numbers.Add(45152);
            foreach (var number in numbers)
            {
                try
            {
                var movie = this.GetMovieInfo(number);
                concurentBag.Add(movie);
            }
            catch { }
            }

            foreach (var movie in concurentBag)
            {
                //check if category exists
                List<int> categoriesIds = await this.GetOrCreateCategoryAsync(movie.CategoryName);
                List<int> artistsIds = await this.GetOrCreateArtistAsync(movie.Artists);
                int directorId = await this.GetOrCreateDirectorAsync(movie.Director);

                var movieExists = this.moviesRepository
                    .AllAsNoTracking()
                    .Any(x => x.Name == movie.MovieName);
                if (movieExists)
                {
                    continue;
                }

                var newMovie = new Movie()
                {
                    Name = movie.MovieName,
                    Year = movie.Year,
                    Runtime = movie.Runtime,
                    PosterPath = movie.PosterPath,
                    SecondPosterPath = movie.SecondPosterPath,
                    TrailerUrl = movie.TrailerUrl,
                    Storyline = movie.Storyline,
                    ImdbRating = movie.ImdbRating,
                    DirectorId = directorId,
                };
                await this.moviesRepository.AddAsync(newMovie);
                await this.moviesRepository.SaveChangesAsync();

                foreach (var artistId in artistsIds)
                {
                    var movieArtist = new MoviesArtist
                    {
                        ArtistId = artistId,
                        MovieId = newMovie.Id,
                    };
                    await this.moviesArtistsRepository.AddAsync(movieArtist);
                    await this.moviesArtistsRepository.SaveChangesAsync();
                }

                foreach (var categoryId in categoriesIds)
                {
                    var movieCategory = new MoviesCategory
                    {
                        MovieId = newMovie.Id,
                        CategoryId = categoryId,
                    };
                    await this.moviesCategoriesRepository.AddAsync(movieCategory);
                    await this.moviesCategoriesRepository.SaveChangesAsync();
                }
            }
        }

        private async Task<List<int>> GetOrCreateCategoryAsync(List<string> categories)
        {
            var categoriesId = new List<int>();
            foreach (var category in categories)
            {
                var currentCategory = this.categoriesRepository
                                                .AllAsNoTracking()
                                                .FirstOrDefault(x => x.Name == category);
                if (currentCategory == null)
                {
                    var newCategory = new Category { Name = category, };
                    await this.categoriesRepository.AddAsync(newCategory);
                    await this.categoriesRepository.SaveChangesAsync();
                    categoriesId.Add(newCategory.Id);
                }
                else
                {
                    categoriesId.Add(currentCategory.Id);
                }
            }

            return categoriesId;
        }

        private async Task<List<int>> GetOrCreateArtistAsync(List<string> artists)
        {
            var artistsIds = new List<int>();
            foreach (var artist in artists)
            {
                var currentArtist = this.artistsRepository
                                                .AllAsNoTracking()
                                                .FirstOrDefault(x => x.Name == artist);
                if (currentArtist == null)
                {
                    var newArtist = new Artist { Name = artist, };
                    await this.artistsRepository.AddAsync(newArtist);
                    await this.artistsRepository.SaveChangesAsync();
                    artistsIds.Add(newArtist.Id);
                }
                else
                {
                    artistsIds.Add(currentArtist.Id);
                }
            }

            return artistsIds;
        }

        private async Task<int> GetOrCreateDirectorAsync(string director)
        {
            Director currentDirector = this.directorsRepository
                                            .All()
                                            .FirstOrDefault(x => x.Name == director);
            if (currentDirector == null)
            {
                Director newDirector = new Director { Name = director, };
                await this.directorsRepository.AddAsync(newDirector);
                await this.directorsRepository.SaveChangesAsync();
                return newDirector.Id;
            }

            return currentDirector.Id;
        }

        private MovieDto GetMovieInfo(int id)
        {
            //Parse the document from the content of a response to a virtual request
            string fullId = id.ToString("D" + 7.ToString());
            var urlAddress = $"https://www.imdb.com/title/tt{fullId}/";
            var document = this.context.OpenAsync(urlAddress)
                .GetAwaiter()
                .GetResult();

            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("The requested URL was not found on our server "))
            {
                throw new InvalidOperationException();
            }

            var movie = new MovieDto();

            //artists
            var artists = document.QuerySelectorAll("#titleCast > table > tbody > tr > td")
                .Select(x => x.InnerHtml)
                .ToArray();

            StringBuilder sbInfo = new StringBuilder();
            for (int i = 2; i < artists.Length; i += 4)
            {
                var info = artists[i].Split(' ', 3);
                var artistInfo = "http://imdb.com" + info[1].Substring(5, info[1].IndexOf('>') - 5).Replace("\"", "");
                var artistName = info[2].Substring(0, info[2].IndexOf('\n'));
                sbInfo.AppendLine(artistInfo);

                movie.Artists.Add(artistName);
            }

            //movieName,movieYear
            var movieInfo = document.QuerySelectorAll("#title-overview-widget > div.vital > div.title_block > div > div.titleBar")
                .Select(x => x.TextContent)
                .ToArray();
            var infoItems = new List<string>();
            var correctInfoItems = movieInfo[0].Split("\n");
            foreach (var item in correctInfoItems)
            {
                var currentItem = item.Trim();
                if (currentItem != "\n"
                    && !string.IsNullOrWhiteSpace(currentItem)
                    && currentItem != "|"
                    && currentItem != "R"
                    && currentItem != "PG-13"
                    && currentItem != "Not Rated"
                    && currentItem != "Unrated"
                    && currentItem != "D"
                    && currentItem != "PG"
                    && currentItem != "G"
                    && currentItem != "Approved"
                    && currentItem != "Passed")
                {
                    infoItems.Add(currentItem);
                }
            }

            var movieInfoArray = infoItems.ToArray();
            string movieNameAndYear = movieInfoArray[0];
            var index = movieNameAndYear.IndexOf('(');

            movie.MovieName = movieNameAndYear.Substring(0, index);
            movie.Year = int.Parse(movieNameAndYear.Substring(index + 1, 4));

            //runtime
            var movieRuntimeInfo = movieInfoArray[1].Split(' ');
            if (movieRuntimeInfo.Length > 1)
            {
                var hourInfo = movieRuntimeInfo[0];
                var minuteInfo = movieRuntimeInfo[1];

                var hour = hourInfo.Substring(0, 1);
                var minute = minuteInfo.Substring(0, minuteInfo.Length - 3);
                movie.Runtime = (int.Parse(hour) * 60) + int.Parse(minute);
            }
            else
            {
                movie.Runtime = int.Parse(movieRuntimeInfo[0]);
            }

            //categories
            for (int i = 2; i < movieInfoArray.Length - 1; i++)
            {
                if (movieInfoArray[i].EndsWith(','))
                {
                    movieInfoArray[i] = movieInfoArray[i].Replace(",", "");
                }

                movie.CategoryName.Add(movieInfoArray[i]);
            }

            //rating
            var raitingInfo = document.QuerySelectorAll("#title-overview-widget > div.vital > div.title_block > div > div.ratings_wrapper > div.imdbRating > div.ratingValue > strong > span")
                           .Select(x => x.TextContent)
                           .ToArray();
            movie.ImdbRating = double.Parse(raitingInfo[0]);

            //poster
            movie.PosterPath = document.QuerySelector("#title-overview-widget > div.vital > div.slate_wrapper > div.poster > a > img").GetAttribute("src");
            //second poster
            movie.SecondPosterPath = "https://www.imdb.com/" + document.QuerySelector("#titleImageStrip > div.mediastrip > a:nth-child(1)").GetAttribute("href");

            //storyline
            var storylineInfo = document.QuerySelectorAll("#titleStoryLine > div:nth-child(3) > p > span")
                           .Select(x => x.TextContent)
                           .ToArray();
            movie.Storyline = storylineInfo[0].TrimStart();

            //trailer
            var trailerInfo = document.QuerySelector("#title-overview-widget > div.vital > div.slate_wrapper > div.slate > a").GetAttribute("href");
            movie.TrailerUrl = "https://www.imdb.com" + trailerInfo;

            //director
            movie.Director = document.QuerySelectorAll("#title-overview-widget > div.plot_summary_wrapper > div.plot_summary > div:nth-child(2) > a")
                .Select(x => x.TextContent)
                .FirstOrDefault();

            return movie;
        }
    }
}

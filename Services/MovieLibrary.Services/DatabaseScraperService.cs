namespace MovieLibrary.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AngleSharp;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Services.Models;
    using MovieLibrary.Web.ViewModels.Movies;
    using Newtonsoft.Json;

    public class DatabaseScraperService : IDatabaseScraperService
    {
        private readonly IConfiguration config;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Artist> artistsRepository;
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<Director> directorsRepository;
        private readonly IRepository<MoviesArtist> moviesArtistsRepository;
        private readonly IRepository<MoviesCategory> moviesCategoriesRepository;
        private IBrowsingContext context;

        public DatabaseScraperService(
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

        public async Task PopulateDbWithMovies(InputMovieForDbViewModel model)
        {
            var concurentBag = new ConcurrentBag<MovieDto>();
            //for (int i = model.StartId; i <= model.EndId; i++)
            //{
            foreach (var number in model.MovieIds)
            {
                try
                {
                    var movie = this.GetMovieInfo(number);
                    concurentBag.Add(movie);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            foreach (var movie in concurentBag)
            {
                try
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

                    if (categoriesIds.Count() == 0)
                    {
                        throw new ArgumentException("List of categories is empty", nameof(categoriesIds));
                    }

                    if (artistsIds.Count() == 0)
                    {
                        throw new ArgumentException("List of artists is empty", nameof(artistsIds));
                    }

                    var newMovie = new Movie()
                    {
                        Name = movie.MovieName,
                        Year = DateTime.Parse(movie.Year).Year,
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private async Task<List<int>> GetOrCreateCategoryAsync(List<Genre> categories)
        {
            var categoriesId = new List<int>();
            foreach (var category in categories)
            {
                var currentCategory = this.categoriesRepository
                                                .AllAsNoTracking()
                                                .FirstOrDefault(x => x.Name == category.Name);
                if (currentCategory == null)
                {
                    var newCategory = new Category { Name = category.Name, };
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

        private async Task<List<int>> GetOrCreateArtistAsync(List<ArtistDto> artists)
        {
            var artistsIds = new List<int>();
            foreach (var artist in artists)
            {
                var currentArtist = this.artistsRepository
                                                .AllAsNoTracking()
                                                .FirstOrDefault(x => x.Name == artist.Name);

                var currentName = artist.Name.Replace(" ", "_");
                var biography = $"https://en.wikipedia.org/wiki/{currentName}";
                var currentPhoto = string.Empty;

                if (artist.PhotoUrl == null)
                {
                    currentPhoto = null;
                }
                else
                {
                    currentPhoto = $"https://image.tmdb.org/t/p/original/{artist.PhotoUrl}";
                }

                if (currentArtist == null)
                {
                    var newArtist = new Artist
                    {
                        Name = artist.Name,
                        PhotoUrl = currentPhoto,
                        BiographyUrl = biography,
                    };
                    await this.artistsRepository.AddAsync(newArtist);
                    await this.artistsRepository.SaveChangesAsync();
                    artistsIds.Add(newArtist.Id);
                }
                else
                {
                    if (currentArtist.PhotoUrl == null)
                    {
                        currentArtist.PhotoUrl = $"https://image.tmdb.org/t/p/original/{artist.PhotoUrl}";
                    }

                    if (currentArtist.BiographyUrl == null)
                    {
                        currentArtist.BiographyUrl = biography;
                    }

                    this.artistsRepository.Update(currentArtist);
                    await this.artistsRepository.SaveChangesAsync();
                    artistsIds.Add(currentArtist.Id);
                }
            }

            return artistsIds;
        }

        private async Task<int> GetOrCreateDirectorAsync(string director)
        {
            if (director == null)
            {
                throw new ArgumentNullException("Director's name is null", nameof(director));
            }

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
            var movie = new MovieDto();
            string urlAddressForDetails = $"https://api.themoviedb.org/3/movie/{id}?api_key=4377b7bce242b8c362afce3ecebe1306";

            // Create a request for the URL.
            WebRequest request = WebRequest.Create(urlAddressForDetails);

            // Get the response.
            WebResponse response = request.GetResponse();

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                MovieDto currentMovie = JsonConvert.DeserializeObject(responseFromServer, typeof(MovieDto)) as MovieDto;
                movie.MovieName = currentMovie.MovieName;
                movie.Year = currentMovie.Year;
                movie.Storyline = currentMovie.Storyline;
                movie.Runtime = currentMovie.Runtime;
                movie.PosterPath = $"https://image.tmdb.org/t/p/original/{currentMovie.PosterPath}";
                if (currentMovie.SecondPosterPath == null)
                {
                    movie.SecondPosterPath = null;
                }
                else
                {
                    movie.SecondPosterPath = $"https://image.tmdb.org/t/p/original/{currentMovie.SecondPosterPath}";
                }

                movie.ImdbId = currentMovie.ImdbId;
                movie.CategoryName = currentMovie.CategoryName;
                if (currentMovie.MovieName == null || currentMovie.Year == null
                    || currentMovie.Storyline == null || currentMovie.Runtime == 0
                    || currentMovie.PosterPath == null || currentMovie.ImdbId == null || currentMovie.CategoryName == null)
                {
                    throw new ArgumentNullException("Some argument is not correct");
                }
            }

            string urlAddressForTrailer = $"https://api.themoviedb.org/3/movie/{id}/videos?api_key=4377b7bce242b8c362afce3ecebe1306";
            request = WebRequest.Create(urlAddressForTrailer);
            response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                TrailerDto trailer = JsonConvert.DeserializeObject(responseFromServer, typeof(TrailerDto)) as TrailerDto;
                var currentTrailer = trailer.TrailerResults.FirstOrDefault().TrailerKey;
                movie.TrailerUrl = $"https://www.youtube.com/embed/{currentTrailer}";
                if (currentTrailer == null)
                {
                    throw new ArgumentNullException("Trailer key is empty", nameof(currentTrailer));
                }
            }

            string urlAddressForDirectorAndArtists = $"https://api.themoviedb.org/3/movie/{id}/credits?api_key=4377b7bce242b8c362afce3ecebe1306";
            request = WebRequest.Create(urlAddressForDirectorAndArtists);
            response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                CrewAndArtistsDto crewAndArtist = JsonConvert.DeserializeObject(responseFromServer, typeof(CrewAndArtistsDto)) as CrewAndArtistsDto;
                var director = new Director();
                director.Name = crewAndArtist.Crew
                                             .Where(x => x.Job == "Director")
                                             .Select(x => x.Name)
                                             .FirstOrDefault();
                movie.Director = director.Name;
                movie.Artists = crewAndArtist.Artists
                                             .Take(15)
                                             .ToList();
                if (director.Name == null)
                {
                    //var currentCreator = crewAndArtist.Crew
                    //                         .Where(x => x.Job == "Creator")
                    //                         .Select(x => x.Name)
                    //                         .FirstOrDefault();
                    //if (currentCreator == null)
                    //{
                        throw new ArgumentNullException("Director's name is null", nameof(director.Name));
                    //}
                    //else
                    //{
                    //    director.Name = currentCreator;
                    //}
                }

                if (movie.Artists == null)
                {
                    throw new ArgumentNullException("List of artist is empty", nameof(movie.Artists));
                }
            }

            // Close the response.
            response.Close();

            var imdbUrl = $"https://www.imdb.com/title/{movie.ImdbId}/";
            var document = this.context.OpenAsync(imdbUrl)
                .GetAwaiter()
                .GetResult();


            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("The requested URL was not found on our server "))
            {
                throw new InvalidOperationException();
            }

            //rating
            var ratingInfo = document.QuerySelectorAll("#title-overview-widget > div.vital > div.title_block > div > div.ratings_wrapper > div.imdbRating > div.ratingValue > strong > span")
                           .Select(x => x.TextContent)
                           .FirstOrDefault();
            if (ratingInfo == null)
            {
                movie.ImdbRating = 0;
            }
            else
            {
                movie.ImdbRating = double.Parse(ratingInfo);
            }

            return movie;
        }
    }
}

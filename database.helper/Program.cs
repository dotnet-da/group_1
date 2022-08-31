// See https://aka.ms/new-console-template for more information
using database.helper.Entitites;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace database.helper // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static string _apiKey = "";
        static MediaServiceContextFactory _dbContextFactory = new MediaServiceContextFactory();
        static MediaServiceContext _dbContext;
        public static HttpClient _client;

        public static int _updateNumberMovies = 0;
        public static int _updateNumberSeries = 0;

        public static DateTime _start;
        static async Task Main(string[] args)
        {
            string username, password, apiKey;
            DateTime start;

            Console.Write("Update Number of Movies: ");
            _updateNumberMovies = int.Parse(Console.ReadLine());
            Console.Write("Update Number of Series: ");
            _updateNumberSeries = int.Parse(Console.ReadLine());

            _start = DateTime.Now;

            try
            {
                username = args[0];
                password = args[1];
                apiKey = args[2];

                //Console.WriteLine("Arguments: username=" + username + ", password=" + password + ", apiKey=" + apiKey);
            }
            catch (Exception)
            {
                Console.WriteLine("Arguments parsing went wrong. Configure them manually: ");
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();
                Console.Write("ApiKey: ");
                apiKey = Console.ReadLine();
            }

            _apiKey = apiKey;

            // Get data from TMDB API
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                                 new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var movieIds = await GetAllMovieIds(1, _updateNumberMovies);
            List<Movie> movies = await GetAllMovies(movieIds);

            var seriesIds = await GetAllSeriesIds(1, _updateNumberSeries);
            List<Series> series = await GetAllSeries(seriesIds);

            Console.WriteLine("Amount of movies: " + movies.Count);
            Console.WriteLine("Amount of series: " + series.Count);

            StartDatabase(username, password);

            FillMediaTableWithMovies(movies);
            FillMediaTableWithSeries(series);

            Console.WriteLine("Finished database update.");
            var end = (DateTime.Now - _start).TotalSeconds;
            Console.WriteLine("Elapsed Time: " + end + "s");
            Console.WriteLine("Elapsed Time in min: " + end / 60 + "min");
            Console.WriteLine("Media per Second: " + (_updateNumberMovies + _updateNumberSeries) / end);
            Console.WriteLine("Media per Minute: " + (_updateNumberMovies + _updateNumberSeries) / (end / 60));
            Console.WriteLine("Media per Hour: " + (_updateNumberMovies + _updateNumberSeries) / (end / 3600));

        }

        public static void StartDatabase(string username, string password)
        {
            // Connect to database
            _dbContext = _dbContextFactory.CreateDbContext(new string[] { username, password });

            Console.WriteLine("Connected.");
        }

        public static void EnsureDatabaseStructure()
        {

            Console.WriteLine("Ensuring database structure...");
            _dbContext.Database.EnsureCreated();
        }

        public static void FillMediaTableWithMovies(List<Movie> movies)
        {
            int counter = 0;
            int batchSize = 100;
            foreach (var movie in movies)
            {
                var movieEntity = _dbContext.Movies.FirstOrDefault(u => u.TmdbId == movie.TmdbId);
                if (movieEntity != null)
                {
                    movieEntity = movie;
                }
                else
                {
                    _dbContext.Movies.Add(movie);
                }

                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r" + "Movie #" + (++counter) + " of " + movies.Count + " -> TmdbId: " + movie.TmdbId + ", Title: " + movie.Title);

                if (counter % batchSize == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Saving a batch of " + batchSize + " entities...");
                    _dbContext.SaveChanges();
                }
            }
            Console.WriteLine();
            Console.WriteLine("Saving the rest...");
            _dbContext.SaveChanges();
        }

        public static void FillMediaTableWithSeries(List<Series> seriesList)
        {
            int counter = 0;
            int batchSize = 100;
            foreach (var series in seriesList)
            {
                var seriesEntity = _dbContext.Series.FirstOrDefault(u => u.TmdbId == series.TmdbId);
                if (seriesEntity != null)
                {
                    seriesEntity = series;
                }
                else
                {
                    _dbContext.Series.Add(series);
                }

                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r" + "Series #" + (++counter) + " of " + seriesList.Count + " -> TmdbId: " + series.TmdbId + ", Title: " + series.Title);


                if (counter % batchSize == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Saving a batch of " + batchSize + " entities...");
                    _dbContext.SaveChanges();
                }
            }
            Console.WriteLine();
            Console.WriteLine("Saving the rest...");
            _dbContext.SaveChanges();
        }

        public static async Task<List<int>> GetAllMovieIds(int pageStart, int amountOfMovies)
        {
            const int pageSize = 20;

            List<int> allMovieIds = new List<int>();

            string path;
            for (int i = 0; i < amountOfMovies / pageSize; i++)
            {
                path = $"https://api.themoviedb.org/3/movie/popular?page={pageStart + i}";


                HttpResponseMessage response = await _client.GetAsync(path);


                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error in GetAllMovieIds: '" + path + "':" + response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);

                if ((joResponse["results"] as JArray).Count > 0)
                {
                    foreach (var obj in (joResponse["results"] as JArray))
                    {
                        allMovieIds.Add(int.Parse(obj["id"].ToString()));
                    }
                }
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r"
                + "Page: " + joResponse["page"] + " of " + ((amountOfMovies / pageSize) + pageStart - 1) + ", Data amount:" + allMovieIds.Count);
            }
            Console.WriteLine();

            return allMovieIds;
        }

        public static async Task<List<int>> GetAllSeriesIds(int pageStart, int amountOfSeries)
        {
            const int pageSize = 20;

            List<int> allSeriesIds = new List<int>();

            string path;
            for (int i = 0; i < amountOfSeries / pageSize; i++)
            {
                path = $"https://api.themoviedb.org/3/tv/popular?page={pageStart + i}";

                HttpResponseMessage response = await _client.GetAsync(path);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error in GetAllSeriesIds: '" + path + "':" + response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);


                if ((joResponse["results"] as JArray).Count > 0)
                {
                    foreach (var obj in (joResponse["results"] as JArray))
                    {
                        allSeriesIds.Add(int.Parse(obj["id"].ToString()));
                    }

                }
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r"
                    + "Page: " + joResponse["page"] + " of " + ((amountOfSeries / pageSize) + pageStart - 1) + ", Data amount:" + allSeriesIds.Count);
            }
            Console.WriteLine();

            return allSeriesIds;
        }

        public static async Task<List<Movie>> GetAllMovies(List<int> movieIds)
        {
            List<Movie> allMovies = new List<Movie>();
            DateTime start;

            Console.WriteLine("Retrieving of details from API...");

            string path;
            start = DateTime.Now;
            int counter = 0;
            foreach (var id in movieIds)
            {
                path = $"https://api.themoviedb.org/3/movie/{id}";


                HttpResponseMessage response = await _client.GetAsync(path);


                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error in GetAllMovies: '" + path + "':" + response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);
                var obj = await ParseJObjectToMovie(joResponse);
                ++counter;
                if (obj != null)
                {
                    allMovies.Add(obj);
                }
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r" + counter + " of " + movieIds.Count + " -> Title: " + obj.Title);
            }
            Console.WriteLine();

            Console.WriteLine("  API Call details: ");
            var elapsedTime = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine("\tElapsed time: " + elapsedTime + "s");
            Console.WriteLine("\tAmount of movies: " + movieIds.Count);
            var moviesPerSecond = movieIds.Count / elapsedTime;
            Console.WriteLine("\tMovies per second: " + moviesPerSecond);
            Console.WriteLine("\tMovies per minute: " + moviesPerSecond * 60);
            Console.WriteLine("\tMovies per hour: " + moviesPerSecond * 60 * 60);

            return allMovies;
        }

        public static async Task<List<Series>> GetAllSeries(List<int> seriesIds)
        {
            List<Series> allSeries = new List<Series>();
            DateTime start;

            Console.WriteLine("Retrieving of details from tv API...");

            string path;
            start = DateTime.Now;
            int counter = 0;
            foreach (var id in seriesIds)
            {
                path = $"https://api.themoviedb.org/3/tv/{id}";


                HttpResponseMessage response = await _client.GetAsync(path);


                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error in GetAllSeries: '" + path + "':" + response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);
                var obj = await ParseJObjectToSeries(joResponse);
                ++counter;
                if (obj != null)
                {
                    allSeries.Add(obj);
                    Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r" + counter + " of " + seriesIds.Count + " -> Title: " + obj.Title + ", Seasons: " + obj.Seasons.Count);
                }
            }
            Console.WriteLine();

            Console.WriteLine("  API Call details: ");
            var elapsedTime = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine("\tElapsed time: " + elapsedTime + "s");
            Console.WriteLine("\tAmount of movies: " + seriesIds.Count);
            var seriesPerSecond = seriesIds.Count / elapsedTime;
            Console.WriteLine("\tSeries per second: " + seriesPerSecond);
            Console.WriteLine("\tSeries per minute: " + seriesPerSecond * 60);
            Console.WriteLine("\tSeries per hour: " + seriesPerSecond * 60 * 60);

            return allSeries;
        }

        public static async Task<List<StreamingInfo>> GetAllStreamingInfos(string type, int id)
        {
            List<StreamingInfo> allStreamingInfos = new List<StreamingInfo>();
            DateTime start;

            string path;

            path = $"https://api.themoviedb.org/3/{type}/{id}/watch/providers";

            HttpResponseMessage response = await _client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error in GetAllStreamingInfos: '" + path + "':" + response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            JObject joResponse = JObject.Parse(content);

            var countries = new List<string>();
            countries.Add("DE");
            countries.Add("FI");
            countries.Add("US");
            try
            {
                foreach (var country in countries)
                {
                    var flatrateArray = (joResponse["results"] as JObject)[country]["flatrate"] as JArray;
                    if (flatrateArray != null)
                    {
                        foreach (JObject obj in flatrateArray)
                        {
                            allStreamingInfos.Add(ParseJObjectToStreamingInfo(obj, country));
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return allStreamingInfos;
        }

        public static StreamingInfo ParseJObjectToStreamingInfo(JObject obj, string country)
        {
            return new StreamingInfo
            {
                Name = (string)obj["provider_name"],
                Country = country
            };
        }

        public static List<Genre> GetGenres(JArray jGenres)
        {
            List<Genre> genres = new List<Genre>();
            foreach (JObject obj in jGenres)
            {
                Genre newGenre = new Genre();
                newGenre.Id = (int)obj["id"];
                newGenre.Name = (string)obj["name"];
                if (newGenre.Id != null && newGenre.Name != null)
                {
                    genres.Add(newGenre);
                }
            }
            return genres;
        }
        public static async Task<Movie> ParseJObjectToMovie(JObject obj)
        {
            try
            {
                Movie movie = new Movie
                {
                    ImdbId = (string)obj["imdb_id"],
                    Rating = (float)obj["vote_average"],
                    VoteCount = (int)obj["vote_count"],
                    TmdbId = (int)obj["id"],
                    Title = (string)obj["title"],
                    Tagline = (string)obj["tagline"],
                    Description = (string)obj["overview"],
                    Release = (string)obj["release_date"] != "" ? ((DateTime)obj["release_date"]).ToUniversalTime() : null,
                    BackdropURL = (string)obj["backdrop_path"],
                    StreamingInfos = await GetAllStreamingInfos("movie", (int)obj["id"]),
                    Runtime = (int)obj["runtime"],
                    Genres = GetGenres((JArray)obj["genres"]),
                };

                return movie;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<Series> ParseJObjectToSeries(JObject obj)
        {
            try
            {
                DateTime? release, lastAirDate;
                try
                {
                    release = (string)obj["first_air_date"] != "" && obj["first_air_date"] != null ? ((DateTime)obj["first_air_date"]).ToUniversalTime() : null;
                }
                catch (Exception)
                {
                    release = null;
                }

                try
                {
                    lastAirDate = (string)obj["last_air_date"] != "" && obj["last_air_date"] != null ? ((DateTime)obj["last_air_date"]).ToUniversalTime() : null;
                }
                catch (Exception)
                {
                    lastAirDate = null;
                }

                Series series = new Series
                {
                    TmdbId = 10000000 + (int)obj["id"],
                    Title = (string)obj["name"],
                    Tagline = (string)obj["tagline"],
                    Description = (string)obj["overview"],
                    Release = release,
                    BackdropURL = (string)obj["backdrop_path"],
                    StreamingInfos = await GetAllStreamingInfos("tv", (int)obj["id"]),
                    LastAirDate = lastAirDate,
                    Seasons = await GetAllSeasons((int)obj["id"], (JArray)obj["seasons"]),
                    Genres = GetGenres((JArray)obj["genres"]),
                };

                return series;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static async Task<List<Season>> GetAllSeasons(int seriesId, JArray seasons)
        {
            List<Season> allSeasons = new List<Season>();

            foreach (var season in seasons)
            {
                string path;

                path = $"https://api.themoviedb.org/3/tv/{seriesId}/season/{(string)season["season_number"]}";

                HttpResponseMessage response = await _client.GetAsync(path);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error in GetAllSeasons: '" + path + "':" + response.StatusCode);
                    continue;
                }

                var content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);

                try
                {
                    allSeasons.Add(ParseJObjectToSeason(joResponse));
                }
                catch (Exception)
                {

                }
            }

            return allSeasons;
        }

        public static Season ParseJObjectToSeason(JObject obj)
        {
            var result = new Season();

            result.Id = (int)obj["id"];
            result.Description = (string)obj["overview"];
            result.Number = (int)obj["season_number"];

            List<Episode> episodes = new List<Episode>();
            foreach (var episode in (JArray)obj["episodes"])
            {
                episodes.Add(new Episode
                {
                    Id = (int)episode["id"],
                    Number = (int)episode["episode_number"],
                    Title = (string)episode["name"],
                    AirDate = (string)obj["air_date"] != "" ? ((DateTime)obj["air_date"]).ToUniversalTime() : null,
                    Rating = (float)episode["vote_average"],
                    VoteCount = (int)episode["vote_count"],
                    StillPath = (string)episode["still_path"],
                });
            }
            result.Episodes = episodes;

            return result;
        }
    }
}


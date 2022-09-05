using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StreamKing.Data.Accounts;
using StreamKing.Data.Media;
using StreamKing.Database.Helper.Models;
using StreamKing.Web.Helpers;
using StreamKing.Web.Models;
using StreamKing.Web.Services;
using System.Text;

namespace StreamKing.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private IMediaService _mediaService;

        private static IMediaServiceContext _mediaServiceContext;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
            _mediaServiceContext = _mediaService.MediaServiceContext;
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ MEDIA ENDPOINTS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        [HttpGet]
        public IActionResult GetAll(string? type, int? take)
        {
            var mediaList = _mediaService.GetAll(type, take);
            return Content(mediaList.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize(AccountType.Admin)]
        [HttpPost]
        public IActionResult AddMedia(Media media)
        {
            var status = _mediaService.AddMedia(media);
            return Ok(status);
        }

        [HttpGet("{id}")]
        public IActionResult GetMediaById([FromRoute] int id)
        {
            var media = _mediaService.GetById(id);
            return Content(media.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize(AccountType.Admin)]
        [HttpPut("{id}")]
        public IActionResult UpdateMediaById([FromRoute] int tmdbId, Media media)
        {
            var status = _mediaService.UpdateById(tmdbId, media);
            return Ok(status);
        }

        [AmsAuthorize(AccountType.Admin)]
        [HttpDelete("{id}")]
        public IActionResult DeleteMediaById([FromRoute] int tmdbId)
        {
            var status = _mediaService.DeleteById(tmdbId);
            return Ok(status);
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ SEASON ENDPOINTS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        [HttpGet("{id}/seasons")]
        public IActionResult GetSeasonById([FromRoute] int id)
        {
            JArray result = new JArray();

            var media = _mediaServiceContext.Series
                .Where(series => series.TmdbId == id)
                .Include(series => series.Seasons)
                .ThenInclude(season => season.Episodes)
                .FirstOrDefault();

            if (media != null)
            {
                result = JArray.FromObject(media.Seasons);
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        [HttpGet("{id}/seasons/{seasonId}")]
        public IActionResult GetSeasonById([FromRoute] int id, [FromRoute] int seasonId)
        {
            JObject result = new JObject();

            var media = _mediaServiceContext.Series
                .Where(series => series.TmdbId == id)
                .Include(series => series.Seasons.Where(s => s.Id == seasonId))
                .ThenInclude(season => season.Episodes)
                .FirstOrDefault();

            if (media != null && media.Seasons.FirstOrDefault() != null)
            {
                result = JObject.FromObject(media.Seasons.First());
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ EPISODE ENDPOINTS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        [HttpGet("{id}/seasons/{seasonId}/episodes/{episodeId}")]
        public IActionResult GetEpisodeById([FromRoute] int id, [FromRoute] int seasonId, [FromRoute] int episodeId)
        {
            JObject result = new JObject();

            var media = _mediaServiceContext.Series
                .Where(series => series.TmdbId == id)
                .Include(series => series.Seasons.Where(s => s.Id == seasonId))
                .ThenInclude(season => season.Episodes.Where(e => e.Id == episodeId))
                .FirstOrDefault();

            if (media != null && media.Seasons.FirstOrDefault() != null && media.Seasons.FirstOrDefault().Episodes.FirstOrDefault() != null)
            {
                result = JObject.FromObject(media.Seasons.First().Episodes.First());
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ WATCHLISTS ENDPOINTS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        [AmsAuthorize(AccountType.Admin)]
        [HttpGet("users/{userId}/watchlists")]
        public IActionResult GetAllWatchlists([FromRoute] Guid userId)
        {
            JArray result = new JArray();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == userId)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.MovieList)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.SeriesList)
                .FirstOrDefault();

            if (user != null && user.Watchlists != null)
            {
                result = JArray.FromObject(user.Watchlists);
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize(AccountType.Admin)]
        [HttpPost("users/{userId}/watchlists")]
        public IActionResult AddWatchlist([FromRoute] Guid userId, Watchlist watchlist)
        {
            JArray result = new JArray();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == userId)
                .Include(acc => acc.Watchlists)
                .FirstOrDefault();

            if (user != null)
            {
                if (user.Watchlists == null) user.Watchlists = new List<Watchlist>();

                user.Watchlists.Add(watchlist);
                _mediaServiceContext.SaveChanges();

                result = JArray.FromObject(user.Watchlists);
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize(AccountType.Admin)]
        [HttpGet("users/{userId}/watchlists/{watchlistId}")]
        public IActionResult GetWatchlist([FromRoute] Guid userId, [FromRoute] int watchlistId)
        {
            JObject result = new JObject();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == userId)
                .Include(acc => acc.Watchlists.Where(wl => wl.Id == watchlistId))
                .ThenInclude(wl => wl.MovieList)
                .ThenInclude(sl => sl.Movie)
                .Include(acc => acc.Watchlists.Where(wl => wl.Id == watchlistId))
                .ThenInclude(wl => wl.SeriesList)
                .ThenInclude(sl => sl.Series)
                .FirstOrDefault();

            if (user != null && user.Watchlists.FirstOrDefault() != null)
            {
                result = JObject.FromObject(user.Watchlists.First());
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize(AccountType.Admin)]
        [HttpPut("users/{userId}/watchlists/{watchlistId}")]
        public IActionResult AddWatchlist([FromRoute] Guid userId, [FromRoute] int watchlistId, Watchlist watchlist)
        {
            JObject result = new JObject();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == userId)
                .Include(acc => acc.Watchlists.Where(wl => wl.Id == watchlistId))
                .FirstOrDefault();

            if (user != null && user.Watchlists.FirstOrDefault() != null)
            {
                watchlist.Id = watchlistId;
                user.Watchlists[0].Name = watchlist.Name;
                user.Watchlists[0].Description = watchlist.Description;
                _mediaServiceContext.SaveChanges();
                result = JObject.FromObject(user.Watchlists.First());
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize(AccountType.Admin)]
        [HttpDelete("users/{userId}/watchlists/{watchlistId}")]
        public IActionResult AddWatchlist([FromRoute] Guid userId, [FromRoute] int watchlistId)
        {
            bool status = false;

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == userId)
                .Include(acc => acc.Watchlists.Where(wl => wl.Id == watchlistId))
                .FirstOrDefault();

            if (user != null && user.Watchlists.FirstOrDefault() != null)
            {
                user.Watchlists.Remove(user.Watchlists[0]);
                _mediaServiceContext.SaveChanges();
                status = true;
            }
            else
            {
                return NotFound();
            }
            return Ok(status);
        }

        [AmsAuthorize]
        [HttpGet("session/watchlists")]
        public IActionResult GetWatchlistsOfCurrentUser()
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            JArray result = new JArray();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == sessionUser.Id)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.MovieList)
                .ThenInclude(sl => sl.Movie)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.SeriesList)
                .ThenInclude(sl => sl.Series)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.SeriesList)
                .ThenInclude(sl => sl.Seasons)
                .ThenInclude(se => se.Season)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.SeriesList)
                .ThenInclude(sl => sl.Seasons)
                .ThenInclude(se => se.Episodes)
                .ThenInclude(e => e.Episode)
                .FirstOrDefault();

            if (user != null && user.Watchlists != null)
            {
                result = JArray.FromObject(user.Watchlists);
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize]
        [HttpGet("session/watchlists/{watchlistId}")]
        public IActionResult GetWatchlistOfCurrentUserById([FromRoute] int watchlistId)
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            JObject result = new JObject();

            var watchlist = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == sessionUser.Id)
                .Include(acc => acc.Watchlists.Where(w => w.Id == watchlistId))
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.MovieList)
                .ThenInclude(sl => sl.Movie)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.SeriesList)
                .ThenInclude(sl => sl.Series)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.SeriesList)
                .ThenInclude(sl => sl.Seasons)
                .ThenInclude(se => se.Season)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.SeriesList)
                .ThenInclude(sl => sl.Seasons)
                .ThenInclude(se => se.Episodes)
                .ThenInclude(e => e.Episode)
                .Select(acc => acc.Watchlists)
                .FirstOrDefault()
                .FirstOrDefault();

            if (watchlist != null)
            {
                result = JObject.FromObject(watchlist);
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize]
        [HttpPost("session/watchlists")]
        public IActionResult AddWatchlistToCurrentUser(Watchlist watchlist)
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            JArray result = new JArray();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == sessionUser.Id)
                .Include(acc => acc.Watchlists)
                .FirstOrDefault();

            if (user != null)
            {
                if (user.Watchlists == null) user.Watchlists = new List<Watchlist>();

                user.Watchlists.Add(watchlist);
                _mediaServiceContext.SaveChanges();

                result = JArray.FromObject(user.Watchlists);
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        // Acts as post and put in one
        [AmsAuthorize]
        [HttpPost("session/watchlists/{watchlistId}/entries")]
        public IActionResult AddMovieEntryToSessionUserWatchlist([FromRoute] int watchlistId, WatchEntryRequest watchEntryRequest)
        {
            // validate watchentryrequest object
            if (watchEntryRequest.SeasonId != null && watchEntryRequest.SeriesId == null) return BadRequest("Please specify SeriesId and SeasonId");
            if (watchEntryRequest.EpisodeId != null && watchEntryRequest.SeasonId == null) return BadRequest("Please specify SeriesId, SeasonId and EpisodeId");
            if (watchEntryRequest.MovieId == null && watchEntryRequest.SeriesId == null) return BadRequest("Please specify either a MovieId or a SeriesId");

            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            JObject result = new JObject();

            if (watchEntryRequest.MovieId != null)
            {
                var movie = _mediaServiceContext.Media
                    .OfType<Movie>()
                    .Where(media => media.TmdbId == watchEntryRequest.MovieId)
                    .FirstOrDefault();

                if (movie == null)
                {
                    return NotFound("Movie with TmdbId " + watchEntryRequest.MovieId + " not found");
                }



                try
                {
                    var user = _mediaServiceContext.Accounts
                        .Where(acc => acc.Id == sessionUser.Id)
                        .Include(acc => acc.Watchlists.Where(w => w.Id == watchlistId))
                        .ThenInclude(wl => wl.MovieList)
                        .FirstOrDefault();

                    var movielist = user.Watchlists.First().MovieList;

                    MovieEntry movieEntry = movielist.FirstOrDefault(s => s.Movie.TmdbId == movie.TmdbId);

                    if (movieEntry == null)
                    {
                        Console.WriteLine("movieEntry is null, creating a new one...");
                        movieEntry = new MovieEntry
                        {
                            Tag = watchEntryRequest.Tag,
                            Movie = movie
                        };

                        movielist.Add(movieEntry);
                    }

                    _mediaServiceContext.SaveChanges();
                    result = JObject.FromObject(new { status = true });
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(ex.Message);
                }

                return Content(result.ToString(), "application/json", Encoding.UTF8);
            }
            else
            {
                var user = _mediaServiceContext.Accounts
                        .Where(acc => acc.Id == sessionUser.Id)
                        .Include(acc => acc.Watchlists.Where(w => w.Id == watchlistId))
                        .ThenInclude(wl => wl.SeriesList)
                        .ThenInclude(sl => sl.Seasons)
                        .ThenInclude(sl => sl.Episodes)
                        .FirstOrDefault();
                if (user == null) return NotFound("Session User does not exist in database.");
                if (user.Watchlists == null || user.Watchlists.Count == 0) return NotFound("Session User does not have a Watchlist with the id " + watchlistId + ".");

                var serieslist = user.Watchlists.First().SeriesList;

                Series? series;
                Season? season;
                Episode? episode;

                var seriesQuery = _mediaServiceContext.Media
                .OfType<Series>()
                .Where(media => media.TmdbId == watchEntryRequest.SeriesId);

                if (watchEntryRequest.SeasonId == null)
                {
                    series = seriesQuery.FirstOrDefault();
                }
                else if (watchEntryRequest.EpisodeId == null)
                {
                    series = seriesQuery
                        .Include(series => series.Seasons)
                        .FirstOrDefault();
                }
                else
                {
                    series = seriesQuery
                        .Include(series => series.Seasons)
                        .ThenInclude(season => season.Episodes)
                        .FirstOrDefault();
                }

                if (series == null) return NotFound("Series with id " + watchEntryRequest.SeriesId + " not found.");

                SeriesEntry seriesEntry = serieslist.FirstOrDefault(s => s.Series.TmdbId == series.TmdbId);

                if (seriesEntry == null)
                {
                    Console.WriteLine("seriesEntry is null, creating a new one...");
                    seriesEntry = new SeriesEntry
                    {
                        Series = series,
                        Seasons = new List<SeasonEntry>()
                    };

                    serieslist.Add(seriesEntry);
                }

                if (watchEntryRequest.SeasonId != null)
                {
                    season = series.Seasons.Where(s => s.Id == watchEntryRequest.SeasonId).FirstOrDefault();

                    if (season == null) return NotFound("Season with id " + watchEntryRequest.SeasonId + " not found.");

                    SeasonEntry seasonEntry = seriesEntry.Seasons.FirstOrDefault(s => s.Season.Id == season.Id);

                    if (seasonEntry == null)
                    {
                        Console.WriteLine("seasonEntry is null, creating a new one...");
                        seasonEntry = new SeasonEntry
                        {
                            Season = season,
                            Episodes = new List<EpisodeEntry>()
                        };

                        seriesEntry.Seasons.Add(seasonEntry);
                    }


                    if (watchEntryRequest.EpisodeId != null)
                    {
                        if (season.Episodes == null) return NotFound("Season with id " + watchEntryRequest.SeasonId + " does not have any episodes.");

                        episode = season.Episodes.Where(e => e.Id == watchEntryRequest.EpisodeId).FirstOrDefault();
                        if (episode == null) return NotFound("Episode with id " + watchEntryRequest.EpisodeId + " not found.");

                        EpisodeEntry episodeEntry = seasonEntry.Episodes.FirstOrDefault(s => s.Episode.Id == episode.Id);

                        if (episodeEntry == null)
                        {
                            Console.WriteLine("episodeEntry is null, creating a new one...");
                            episodeEntry = new EpisodeEntry
                            {
                                Tag = watchEntryRequest.Tag,
                                Episode = episode,
                            };

                            seasonEntry.Episodes.Add(episodeEntry);
                        }
                    }
                    else
                    {
                        seasonEntry.Tag = watchEntryRequest.Tag;
                    }
                }
                else
                {
                    seriesEntry.Tag = watchEntryRequest.Tag;
                }

                _mediaServiceContext.SaveChanges();
                result = JObject.FromObject(new { status = true });
            }

            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        // Acts as post and put in one
        [AmsAuthorize]
        [HttpDelete("session/watchlists/{watchlistId}/entries/{entryId}")]
        public IActionResult RemoveEntryFromWatchlistOfCurrentUserById([FromRoute] int watchlistId, [FromRoute] int entryId)
        {

            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            var watchlist = _mediaServiceContext.Accounts
                        .Where(acc => acc.Id == sessionUser.Id)
                        .Include(acc => acc.Watchlists.Where(w => w.Id == watchlistId));

            var movielist = watchlist.ThenInclude(wl => wl.MovieList).FirstOrDefault().Watchlists.FirstOrDefault().MovieList;

            if (movielist.FirstOrDefault(m => m.Id == entryId) != null)
            {
                movielist.Remove(movielist.First(m => m.Id == entryId));
                _mediaServiceContext.SaveChanges();
                return Content((JObject.FromObject(new { status = true, message = "Removed MovieEntry with id " + entryId })).ToString(), "application/json", Encoding.UTF8);
            }

            var seriesList = watchlist
                .ThenInclude(wl => wl.SeriesList)
                .ThenInclude(sl => sl.Seasons)
                .ThenInclude(sl => sl.Episodes)
                .FirstOrDefault().Watchlists
                .FirstOrDefault().SeriesList;

            if (seriesList.FirstOrDefault(s => s.Id == entryId) != null)
            {
                seriesList.Remove(seriesList.First(s => s.Id == entryId));
                _mediaServiceContext.SaveChanges();
                return Content((JObject.FromObject(new { status = true, message = "Removed SeriesEntry with id " + entryId })).ToString(), "application/json", Encoding.UTF8);
            }

            return BadRequest();
        }
    }
}

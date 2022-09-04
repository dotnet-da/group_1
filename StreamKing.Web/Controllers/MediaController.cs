using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StreamKing.Data.Accounts;
using StreamKing.Data.Media;
using StreamKing.Database.Helper.Models;
using StreamKing.Web.Helpers;
using StreamKing.Web.Models;
using StreamKing.Web.Services;
using System.Linq;
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

            if (media!= null && media.Seasons.FirstOrDefault() != null)
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
        public IActionResult GetAllWatchlists([FromRoute]Guid userId)
        {
            JArray result = new JArray();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id== userId)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.MovieList)
                .Include(acc => acc.Watchlists)
                .ThenInclude(wl => wl.SeriesList)
                .FirstOrDefault();

            if (user != null && user.Watchlists!= null)
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

        [AmsAuthorize]
        [HttpPost("session/watchlists/{watchlistId}/movies")]
        public IActionResult AddMovieEntryToSessionUserWatchlist([FromRoute]int watchlistId, MovieEntryRequest movieEntryRequest)
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            var movie = _mediaServiceContext.Media
                .OfType<Movie>()
                .Where(media => media.TmdbId == movieEntryRequest.MovieId)
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound("Movie with TmdbId " + movieEntryRequest.MovieId + " not found");
            }

            MovieEntry movieEntry = new MovieEntry {
                Tag = movieEntryRequest.Tag,
                Movie = movie
            };

            JObject result = new JObject();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == sessionUser.Id)
                .Include(acc => acc.Watchlists.Where(wl => wl.Id == watchlistId))
                .FirstOrDefault();

            if (user != null && user.Watchlists.FirstOrDefault() != null)
            {
                user.Watchlists[0].MovieList.Add(movieEntry);
                _mediaServiceContext.SaveChanges();
                result = JObject.FromObject(movieEntry);
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }

        [AmsAuthorize]
        [HttpPost("session/watchlists/{watchlistId}/series")]
        public IActionResult AddSeriesEntryToSessionUserWatchlist([FromRoute] int watchlistId, SeriesEntry seriesEntry)
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            JObject result = new JObject();

            var user = _mediaServiceContext.Accounts
                .Where(acc => acc.Id == sessionUser.Id)
                .Include(acc => acc.Watchlists.Where(wl => wl.Id == watchlistId))
                .FirstOrDefault();

            if (user != null && user.Watchlists.FirstOrDefault() != null)
            {
                user.Watchlists.First().SeriesList.Add(seriesEntry);
                _mediaServiceContext.SaveChanges();
                result = JObject.FromObject(seriesEntry);
            }
            else
            {
                return NotFound();
            }
            return Content(result.ToString(), "application/json", Encoding.UTF8);
        }
    }
}

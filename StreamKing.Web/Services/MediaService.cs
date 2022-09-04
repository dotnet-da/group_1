using StreamKing.Web.Models;
using StreamKing.Web.Helpers;
using StreamKing.Data.Accounts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using StreamKing.Database.Helper.Models;
using StreamKing.Data.Media;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace StreamKing.Web.Services
{
    public interface IMediaService
    {
        JArray GetAll(string? type, int? take);
        public Media AddMedia(Media media);
        JObject GetById(int tmdbId);
        public JObject GetSeriesWithSeasonById(int tmdbId, int seasonId);
        public bool UpdateById(int tmdbId, Media media);
        public bool DeleteById(int tmdbId);
        public IMediaServiceContext MediaServiceContext { get; set; }
    }

    public class MediaService : IMediaService
    {
        private static MediaServiceContextFactory _mediaServiceContextFactory = new MediaServiceContextFactory();
        public IMediaServiceContext MediaServiceContext { get; set; }

        private readonly AppSettings _appSettings;

        public MediaService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            MediaServiceContext = _mediaServiceContextFactory.CreateDbContext(new string[] {
                _appSettings.LoginUsername,_appSettings.LoginPassword
            });
        }

        public MediaService(IOptions<AppSettings> appSettings, IMediaServiceContext context)
        {
            _appSettings = appSettings.Value;
            MediaServiceContext = context;
        }

        public JArray GetAll(string? type, int? take)
        {
            JArray allMedia = new JArray();
            if(type == null)
            {
                if(take == null)
                {
                    allMedia = JArray.FromObject(MediaServiceContext.Media
                        .Include(media => media.Genres)
                        .Include(media => media.StreamingInfos)
                        .Include(media => (media as Series).Seasons)
                        .ToList());
                }
                else
                {
                    allMedia = JArray.FromObject(MediaServiceContext.Media
                        .Include(media => media.Genres)
                        .Include(media => media.StreamingInfos)
                        .Include(media => (media as Series).Seasons)
                        .Take((int)take)
                        .ToList());
                }
            }
            else
            {
                if (type == "series")
                {
                    if (take == null)
                    {
                        allMedia = JArray.FromObject(MediaServiceContext.Media
                            .OfType<Series>()
                            .Include(media => media.Genres)
                            .Include(media => media.StreamingInfos)
                            .Include(media => media.Seasons)
                            .ToList());
                    }
                    else
                    {
                        allMedia = JArray.FromObject(MediaServiceContext.Media
                            .OfType<Series>()
                            .Include(media => media.Genres)
                            .Include(media => media.StreamingInfos)
                            .Include(media => media.Seasons)
                            .Take((int)take)
                            .ToList());
                    }
                } else if (type == "movie")
                {
                    if (take == null)
                    {
                        allMedia = JArray.FromObject(MediaServiceContext.Media
                            .OfType<Movie>()
                            .Include(media => media.Genres)
                            .Include(media => media.StreamingInfos)
                            .ToList());
                    }
                    else
                    {
                        allMedia = JArray.FromObject(MediaServiceContext.Media
                            .OfType<Movie>()
                            .Include(media => media.Genres)
                            .Include(media => media.StreamingInfos)
                            .Take((int)take)
                            .ToList());
                    }
                }
            }

            return allMedia;
        }
        public Media AddMedia(Media media)
        {
            MediaServiceContext.Media.Add(media);
            MediaServiceContext.SaveChanges();
            return media;
        }

        public JObject GetById(int tmdbId)
        {
            JObject result = new JObject();

            var media = MediaServiceContext.Media
                .Where(media => media.TmdbId == tmdbId)
                .Include(media => media.Genres)
                .Include(media => media.StreamingInfos)
                .Include(media => (media as Series).Seasons)
                .FirstOrDefault();

            if( media != null)
            {
                result = JObject.FromObject(media);
            }
            return result;
        }

        public JObject GetSeriesWithSeasonById(int tmdbId, int seasonId)
        {
            JObject result = new JObject();

            var media = MediaServiceContext.Series
                .Where(series => series.TmdbId == tmdbId)
                .Include(series => series.Seasons.Where(s => s.Id == seasonId))
                .ThenInclude(season => season.Episodes)
                .FirstOrDefault();

            if (media != null)
            {
                result = JObject.FromObject(media);
            }
            return result;
        }

        public bool UpdateById(int tmdbId, Media media)
        {
            if(media.TmdbId != tmdbId) return false;

            var contextMedia = MediaServiceContext.Media.FirstOrDefault(x => x.TmdbId == tmdbId);
            
            if (contextMedia == null) return false;
                
            contextMedia = media;
            MediaServiceContext.SaveChanges();

            return true;
        }

        public bool DeleteById(int tmdbId)
        {
            var contextMedia = MediaServiceContext.Media.FirstOrDefault(x => x.TmdbId == tmdbId);

            if (contextMedia == null) return false;

            MediaServiceContext.Media.Remove(contextMedia);
            MediaServiceContext.SaveChanges();

            return true;
        }
    }
}

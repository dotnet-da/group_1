using StreamKing.Data.Accounts;
using StreamKing.Data.Media;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace StreamKing.Database.Helper.Models
{
    public class MediaServiceContext : DbContext, IMediaServiceContext
    {

        public MediaServiceContext(DbContextOptions<MediaServiceContext> options)
        : base(options)
        {
        }

        public DbSet<Media> Media { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // delete strategies
            modelBuilder.Entity<Account>()
                .HasMany(b => b.Logs)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeasonEntry>()
                .HasMany(b => b.Episodes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeriesEntry>()
                .HasMany(b => b.Seasons)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // inheritance settings
            modelBuilder.Entity<Media>()
                .HasDiscriminator<string>("type")
                .HasValue<Movie>("movie")
            .HasValue<Series>("series");

            modelBuilder.Entity<WatchEntry>()
                .HasDiscriminator<string>("watchEntry_type")
                .HasValue<MovieEntry>("movie")
                .HasValue<SeriesEntry>("series")
                .HasValue<SeasonEntry>("season")
                .HasValue<EpisodeEntry>("episode");
        }

        public int SaveChanges()
        {
            base.SaveChanges();
            return 1;
        }
    }
}

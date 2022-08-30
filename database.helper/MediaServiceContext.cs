using AccountsDLL.Entities;
using database.helper.Entitites;
using Microsoft.EntityFrameworkCore;

namespace database.helper
{
    public class MediaServiceContext : DbContext
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
            // inheritance settings
            modelBuilder.Entity<Media>()
                .HasDiscriminator<string>("type")
                .HasValue<Movie>("movie")
                .HasValue<Series>("series");
        }
    }
}

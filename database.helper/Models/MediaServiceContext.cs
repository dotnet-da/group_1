using AccountsDLL.Entities;
using database.helper.Entitites;
using Microsoft.EntityFrameworkCore;

namespace database.helper.Models
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

            // inheritance settings
            modelBuilder.Entity<Media>()
                .HasDiscriminator<string>("type")
                .HasValue<Movie>("movie")
                .HasValue<Series>("series");
        }

        public int SaveChanges()
        {
            base.SaveChanges();
            return 1;
        }
    }
}

using database.helper.Entitites;
using Microsoft.EntityFrameworkCore;

namespace database.helper
{
    public class MediaServiceContext : DbContext
    {

        private readonly string ConnectionString;
        public MediaServiceContext(string connectionString)
        {
            ConnectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        public DbSet<Media> Media { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }

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

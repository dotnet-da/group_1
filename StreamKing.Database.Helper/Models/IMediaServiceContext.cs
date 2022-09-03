using StreamKing.Data.Media;
using StreamKing.Data.Accounts;
using Microsoft.EntityFrameworkCore;

namespace StreamKing.Database.Helper.Models
{
    public interface IMediaServiceContext : IDisposable
    {
        public DbSet<Media> Media { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Account> Accounts { get; set; }

        int SaveChanges();
    }
}

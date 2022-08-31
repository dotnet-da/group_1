using AccountsDLL.Entities;
using database.helper.Entitites;
using Microsoft.EntityFrameworkCore;

namespace database.helper.Models
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

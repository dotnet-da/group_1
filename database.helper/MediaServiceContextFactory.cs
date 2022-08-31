using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace database.helper
{
    public class MediaServiceContextFactory : IDesignTimeDbContextFactory<MediaServiceContext>
    {
        public static string _username { get; set; }
        public static string _password { get; set; }
        public MediaServiceContext CreateDbContext(string[] args)
        {

            try
            {
                if (_username == null)
                {
                    _username = args[0];
                }
                if (_password == null)
                {
                    _password = args[1];
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Arguments parsing went wrong. Configure them manually: ");
                Console.Write("Username: ");
                _username = Console.ReadLine();
                Console.Write("Password: ");
                _password = Console.ReadLine();
            }

            var connString = $"Host=postgres.fbi.h-da.de;Username={_username};Password={_password};Database=sttoabel;Include Error Detail=true;";

            var optionsBuilder = new DbContextOptionsBuilder<MediaServiceContext>();
            optionsBuilder.UseNpgsql(connString);

            return new MediaServiceContext(optionsBuilder.Options);
        }
    }
}
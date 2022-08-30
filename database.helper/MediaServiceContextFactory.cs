using database.helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class MediaServiceContextFactory : IDesignTimeDbContextFactory<MediaServiceContext>
{
    public MediaServiceContext CreateDbContext(string[] args)
    {
        string username, password;

        try
        {
            username = args[0];
            password = args[1];
        }
        catch (Exception)
        {
            Console.WriteLine("Arguments parsing went wrong. Configure them manually: ");
            Console.Write("Username: ");
            username = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();
        }

        var connString = $"Host=postgres.fbi.h-da.de;Username={username};Password={password};Database=sttoabel;Include Error Detail=true;";

        var optionsBuilder = new DbContextOptionsBuilder<MediaServiceContext>();
        optionsBuilder.UseNpgsql(connString);

        return new MediaServiceContext(optionsBuilder.Options);
    }
}
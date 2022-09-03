using StreamKing.Web.Helpers;
using StreamKing.Web.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddCors();
    services.AddControllers();

    bool login = false;
    int tries = 0;
    string password = "";
    string username = "";
    while (!login)
    {
        Console.WriteLine("Login to database: ");
        Console.Write("Username: ");
        username = Console.ReadLine();
        Console.Write("Password: ");
        {
            // password read function from https://www.c-sharpcorner.com/forums/password-in-c-sharp-console-application
            password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }

            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
        }

        byte[] salt = Encoding.ASCII.GetBytes("hda");

        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashedLogin = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: (username + ":" + password)!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        login = (builder.Configuration.GetSection("AppSettings")["HashedLogin"] == hashedLogin);

        if (!login)
        {
            tries++;
            Console.WriteLine("Wrong username or password. (" + tries + ")");
            if (tries == 3)
            {
                Console.WriteLine("Too many failed tries, stopping backend.");
                return;
            }
        }
    }
    Console.WriteLine("SUCCESSFUL LOGIN.");
    builder.Configuration.GetSection("AppSettings")["LoginUsername"] = username;
    builder.Configuration.GetSection("AppSettings")["LoginPassword"] = password;

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IAccountsManagementService, AccountsManagementService>();

}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.MapControllers();

app.Run("https://localhost:9595");

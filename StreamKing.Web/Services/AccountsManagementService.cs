using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StreamKing.Data.Accounts;
using StreamKing.Database.Helper.Models;
using StreamKing.Web.Helpers;
using StreamKing.Web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace StreamKing.Web.Services
{
    public interface IAccountsManagementService
    {
        RegisterResponse Register(RegisterRequest model);
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        UpdateResponse UpdateAccount(UpdateRequest model);
        DeleteResponse DeleteAccount(Guid id);
        IEnumerable<Account> GetAll();
        Account GetById(Guid id);

        public IMediaServiceContext MediaServiceContext { get; set; }
    }

    public class AccountsManagementService : IAccountsManagementService
    {
        private static MediaServiceContextFactory _mediaServiceContextFactory = new MediaServiceContextFactory();
        public IMediaServiceContext MediaServiceContext { get; set; }

        private readonly AppSettings _appSettings;

        public AccountsManagementService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            MediaServiceContext = _mediaServiceContextFactory.CreateDbContext(new string[] {
                _appSettings.LoginUsername,_appSettings.LoginPassword
            });
        }

        public AccountsManagementService(IOptions<AppSettings> appSettings, IMediaServiceContext context)
        {
            _appSettings = appSettings.Value;
            MediaServiceContext = context;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            byte[] salt = Encoding.ASCII.GetBytes(model.Username);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            model.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            var user = MediaServiceContext.Accounts.SingleOrDefault(x => x.Username == model.Username);

            Console.WriteLine(model.Username + ": " + model.Password);

            if (user == null)
            {
                return null;
            }
            // return null if user not found
            if (user.Password != model.Password)
            {
                user.Log(new AccountLog
                {
                    Timestamp = DateTime.Now.ToUniversalTime(),
                    Level = Data.Accounts.LogLevel.Warning,
                    Source = "AccountsManagementService.Authenticate",
                    Message = "WARNING: failed login."
                });
                user.FailedLogins++;
                MediaServiceContext.SaveChanges();
                return null;
            }

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            user.Log(new AccountLog
            {
                Timestamp = DateTime.Now.ToUniversalTime(),
                Level = Data.Accounts.LogLevel.Info,
                Source = "AccountsManagementService.Authenticate",
                Message = "Successfull user login."
            });
            user.FailedLogins = 0;
            MediaServiceContext.SaveChanges();

            return new AuthenticateResponse(user, token);
        }

        public RegisterResponse Register(RegisterRequest model)
        {
            bool status = true;
            string message = "Successfully registered new user.";
            string token = "";

            var user = MediaServiceContext.Accounts.SingleOrDefault(x => x.Username == model.Username);

            // return null if username already exists
            if (user != null)
            {
                status = false;
                message = "Username already exists.";
                return new RegisterResponse(user, status, message, "");
            }
            else
            {
                // check if password fulfills the requirements
                if (model.Password.Length < 8)
                {
                    status = false;
                    message = "Password must have at least 8 characters.";
                    return new RegisterResponse(user, status, message, "");
                }
                else if (model.Password.Length > 40)
                {
                    status = false;
                    message = "Password cannot have more than 40 characters.";
                    return new RegisterResponse(user, status, message, "");
                }

                byte[] salt = Encoding.ASCII.GetBytes(model.Username);

                // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                model.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: model.Password!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                user = new Account
                {
                    Username = model.Username,
                    Password = model.Password,
                    FirstName = model.FirstName != null ? model.FirstName : "",
                    LastName = model.LastName != null ? model.LastName : "",
                    Region = model.Region != null ? model.Region : "US",
                    Email = model.Email != null ? model.Email : ""
                };

                MediaServiceContext.Accounts.Add(user);
                user.Log(new AccountLog
                {
                    Timestamp = DateTime.Now.ToUniversalTime(),
                    Level = Data.Accounts.LogLevel.Info,
                    Source = "AccountsManagementService.Register",
                    Message = "Created user."
                });

                MediaServiceContext.SaveChanges();

                token = generateJwtToken(user);
            }

            return new RegisterResponse(user, status, message, token);
        }

        public UpdateResponse UpdateAccount(UpdateRequest model)
        {
            bool status = false;
            string message = "Successfully updated following properties: ";

            var user = MediaServiceContext.Accounts.SingleOrDefault(x => x.Id == model.Id);

            if (user == null)
            {
                message = $"Error: user with id '{model.Id}' does not exist.";
            }
            else
            {
                foreach (PropertyInfo prop in model.GetType().GetProperties())
                {
                    var property = prop.Name;
                    var newValue = model.GetType().GetProperty(property).GetValue(model, null);
                    var oldValue = user.GetType().GetProperty(property).GetValue(user, null);
                    if (newValue != null)
                    {
                        if (newValue.ToString() != oldValue.ToString())
                        {
                            if (newValue.GetType() == typeof(DateTime))
                            {
                                newValue = ((DateTime)newValue).ToUniversalTime();
                            }
                            user.GetType().GetProperty(property).SetValue(user, newValue);
                            message += "" + property + "=" + newValue + ", ";
                            status = true;
                        }
                    }
                }
                message = message.Substring(0, message.Length - 2);
                if (!status)
                {
                    message = "Info: did not change any properties.";
                }
                user.Log(new AccountLog
                {
                    Timestamp = DateTime.Now.ToUniversalTime(),
                    Level = Data.Accounts.LogLevel.Info,
                    Source = "AccountsManagementService.UpdateAccount",
                    Message = message,
                });
                MediaServiceContext.SaveChanges();
            }
            return new UpdateResponse(user, status, message);
        }

        public DeleteResponse DeleteAccount(Guid id)
        {
            bool status = false;
            string message = "";

            var user = MediaServiceContext.Accounts.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                message = $"Error: user with id '{id}' does not exist.";
            }
            else
            {
                MediaServiceContext.Accounts.Remove(user);
                MediaServiceContext.SaveChanges();

                status = true;
                message = $"Removed user with id '{id}'";
            }

            return new DeleteResponse(status, message);
        }

        public IEnumerable<Account> GetAll()
        {
            return MediaServiceContext.Accounts;
        }

        public Account GetById(Guid id)
        {
            Console.WriteLine("id: " + id);
            Account user = MediaServiceContext.Accounts.FirstOrDefault(x => x.Id == id);
            user.Log(new AccountLog
            {
                Timestamp = DateTime.Now.ToUniversalTime(),
                Level = Data.Accounts.LogLevel.Info,
                Source = "AccountsManagementService.GetById",
                Message = "Retrieval of account details.",
            });
            MediaServiceContext.SaveChanges();
            return user;
        }

        // helper methods

        private string generateJwtToken(Account user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("auth", Enum.GetName(typeof(AccountType), user.Type)) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

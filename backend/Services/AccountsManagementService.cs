using AccountsDLL.Entities;
using AccountsDLL.Models;
using backend.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public interface IAccountsManagementService
    {
        RegisterResponse Register(RegisterRequest model);
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        UpdateResponse UpdateAccount(UpdateRequest model);
        DeleteResponse DeleteAccount(Guid id);
        IEnumerable<Account> GetAll();
        Account GetById(Guid id);
    }

    public class AccountsManagementService : IAccountsManagementService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private static List<Account> _accounts = new List<Account>
        {
            new Account { Id = new Guid("a54d3db4-8520-4058-b433-cf9b608164d5"), FirstName = "Ad", LastName = "Min", Username = "admin", Password = "admin", Type = AccountType.Admin },
            new Account { Id = new Guid("2184489d-1e7b-40e4-a42e-cc2ddcb2c162"), FirstName = "Test", LastName = "User", Username = "test", Password = "test" }

        };

        private readonly AppSettings _appSettings;

        public AccountsManagementService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _accounts.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public RegisterResponse Register(RegisterRequest model)
        {
            bool status = true;
            string message = "Successfully registered new user.";
            string token = "";

            var user = _accounts.SingleOrDefault(x => x.Username == model.Username);

            // return null if username already exists
            if (user != null)
            {
                status = false;
                message = "Username already exists.";
                return new RegisterResponse(user, status, message, "");
            }
            else
            {
                user = new Account { Id = Guid.NewGuid(), Username = model.Username, Password = model.Password };
                _accounts.Add(user);

                token = generateJwtToken(user);
            }

            return new RegisterResponse(user, status, message, token);
        }

        public UpdateResponse UpdateAccount(UpdateRequest model)
        {
            bool status = false;
            string message = "Successfully updated following properties: ";

            var user = _accounts.SingleOrDefault(x => x.Id == model.Id);

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
                        Console.WriteLine("newValue: " + newValue + ", oldValue: " + oldValue);
                        Console.WriteLine("newValue: " + newValue.GetType() + ", oldValue: " + oldValue.GetType());
                        Console.WriteLine("is different: " + (newValue.ToString() != oldValue.ToString()));
                        if (newValue.ToString() != oldValue.ToString())
                        {
                            user.GetType().GetProperty(property).SetValue(user, newValue);
                            message += "" + property + "=" + newValue + ", ";
                            status = true;
                        }
                    }
                }
                if (!status)
                {
                    message = "Info: did not change any properties.";
                }
            }
            return new UpdateResponse(user, status, message);
        }

        public DeleteResponse DeleteAccount(Guid id)
        {
            bool status = false;
            string message = "";

            var user = _accounts.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                message = $"Error: user with id '{id}' does not exist.";
            }
            else
            {
                _accounts.Remove(user);
                status = true;
                message = $"Removed user with id '{id}'";
            }

            return new DeleteResponse(status, message);
        }

        public IEnumerable<Account> GetAll()
        {
            return _accounts;
        }

        public Account GetById(Guid id)
        {
            Console.WriteLine("id: " + id);
            Console.WriteLine("_accounts:");
            foreach (Account a in _accounts)
            {
                Console.WriteLine(a.Id);
            }

            Account user = _accounts.FirstOrDefault(x => x.Id == id);
            Console.WriteLine("Found user: " + user);
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

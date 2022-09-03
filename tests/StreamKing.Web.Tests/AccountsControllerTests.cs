using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Moq.EntityFrameworkCore;
using StreamKing.Data.Accounts;
using StreamKing.Database.Helper.Models;
using StreamKing.Web.Controllers;
using StreamKing.Web.Helpers;
using StreamKing.Web.Models;
using StreamKing.Web.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace StreamKing.Web.Tests
{
    [TestClass]
    public class AccountsControllerTests
    {
        public static AppSettings _appSettings { get; private set; }
        public static Moq.Mock<IMediaServiceContext> _testMediaServiceContext { get; private set; }
        public static AccountsManagementService _accountsManagementService { get; private set; }
        public static AccountsController _accountsController { get; private set; }


        public static string _testAdminPassword = "admintest";
        public static Account _testAdmin = new Account
        {
            Id = new Guid("0d0ed5c8-11ff-4125-b091-353c07eea569"),
            Username = _testAdminPassword,
            Password = GetHashedPassword(_testAdminPassword, _testAdminPassword),
            Type = AccountType.Admin
        };

        public static string _testUserPassword = "testuser";
        public static Account _testUser = new Account
        {
            Id = new Guid("50f8afbb-eec2-4eb8-b73c-2c914858b0ee"),
            Username = _testUserPassword,
            Password = GetHashedPassword(_testUserPassword, _testUserPassword),
        };

        private string GetToken(AuthenticateRequest request)
        {
            string token = null;
            var response = _accountsController.Authenticate(request);

            if (response.GetType() == typeof(OkObjectResult))
            {
                var okObjectResult = response as OkObjectResult;
                var model = okObjectResult.Value as AuthenticateResponse;

                token = model.Token;
            }

            return token;
        }

        private bool CheckClaimFromToken(string token, string claim, string value)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claimValue = jwtToken.Claims.First(x => x.Type == claim).Value;

            return claimValue == value;
        }

        private static string GetHashedPassword(string username, string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(username);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return password;
        }

        [TestInitialize]
        public void Initialize()
        {
            _appSettings = new AppSettings { Secret = "Test secret-----" };
            IOptions<AppSettings> appSettings = Options.Create(_appSettings);

            _testMediaServiceContext = new Moq.Mock<IMediaServiceContext>();

            IList<Account> accounts = new List<Account>() { _testAdmin, _testUser };
            _testMediaServiceContext.Setup(x => x.Accounts).ReturnsDbSet(accounts);

            _testMediaServiceContext.Setup(i => i.Accounts.Add(It.IsAny<Account>()))
                .Callback((Account item) => { item.Id = Guid.NewGuid(); accounts.Add(item); });

            _testMediaServiceContext.Setup(i => i.Accounts.Remove(It.IsAny<Account>()))
                .Callback((Account item) => accounts.Remove(item));

            _accountsManagementService = new AccountsManagementService(appSettings, _testMediaServiceContext.Object);
            _accountsController = new AccountsController(_accountsManagementService);
        }

        [TestMethod]
        public void AuthenticateUser()
        {
            // Requests for different scenarios
            var adminRequest = new AuthenticateRequest
            {
                Username = _testAdminPassword,
                Password = _testAdminPassword
            };

            var userRequest = new AuthenticateRequest
            {
                Username = _testUserPassword,
                Password = _testUserPassword
            };

            var wrongRequest = new AuthenticateRequest
            {
                Username = "admin",
                Password = "admin2"
            };

            // Act
            var adminResponse = _accountsController.Authenticate(adminRequest);
            var userResponse = _accountsController.Authenticate(userRequest);
            var wrongResponse = _accountsController.Authenticate(wrongRequest);

            // Assert
            Assert.IsInstanceOfType(adminResponse, typeof(OkObjectResult));
            Assert.IsInstanceOfType(userResponse, typeof(OkObjectResult));
            Assert.IsInstanceOfType(wrongResponse, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void AuthorizationLevels()
        {
            // Requests for different scenarios
            var adminRequest = new AuthenticateRequest
            {
                Username = _testAdminPassword,
                Password = _testAdminPassword
            };

            var userRequest = new AuthenticateRequest
            {
                Username = _testUserPassword,
                Password = _testUserPassword
            };

            // Act
            var adminToken = GetToken(adminRequest);
            var userToken = GetToken(userRequest);

            var adminAuthLevel = Enum.GetName(typeof(AccountType), AccountType.Admin);
            var userAuthLevel = Enum.GetName(typeof(AccountType), AccountType.User);

            // Assert
            Assert.IsTrue(CheckClaimFromToken(adminToken, "auth", adminAuthLevel));
            Assert.IsTrue(CheckClaimFromToken(userToken, "auth", userAuthLevel));
        }

        [TestMethod]
        public void GetAccounts()
        {
            // Act & Parse
            var responseAll = _accountsController.GetAll();
            var okObjectResultAll = responseAll as OkObjectResult;
            var modelAll = okObjectResultAll.Value as IEnumerable<Account>;

            var responseUser = _accountsController.GetAccount(_testUser.Id);
            var okObjectResultUser = responseUser as OkObjectResult;
            var modelUser = okObjectResultUser.Value as Account;

            // Assert
            Assert.IsTrue(modelAll.Count() > 1);
            Assert.IsTrue(modelUser.Username == _testUser.Username);
        }

        [TestMethod]
        public void UpdateAccount()
        {
            // UserId for specific user api
            var updateRequest = new UpdateRequest { Email = "test@test.com" };

            // Act & Parse
            var response = _accountsController.UpdateAccount(_testUser.Id, updateRequest);
            var okObjectResult = response as OkObjectResult;
            var model = okObjectResult.Value as UpdateResponse;

            // Assert
            Assert.IsTrue(model.UpdatedAccount.Email == updateRequest.Email);
        }

        [TestMethod]
        public void RegisterAndDeleteAccount()
        {
            // Prepare test user to be deleted
            var testUserRegisterRequest = new RegisterRequest
            {
                Username = "todelete",
                Password = "todelete"
            };

            var userToBeDeletedModel = (_accountsController.Register(testUserRegisterRequest) as OkObjectResult).Value as RegisterResponse;
            var user = userToBeDeletedModel.User;

            // Act & Parse
            var responseAll = _accountsController.GetAll();
            var okObjectResultAll = responseAll as OkObjectResult;
            var modelAll = okObjectResultAll.Value as IEnumerable<Account>;

            Assert.IsTrue(userToBeDeletedModel.Status);

            // Act & Parse
            var response = _accountsController.DeleteAccount(user.Id);
            var okObjectResult = response as OkObjectResult;
            var model = okObjectResult.Value as DeleteResponse;

            var allAccounts = (_accountsController.GetAll() as OkObjectResult).Value as IEnumerable<Account>;
            var findUser = allAccounts.SingleOrDefault(x => x.Id == user.Id);

            // Assert
            Assert.IsTrue(model.Status);
            Assert.IsNull(findUser);
        }
    }
}
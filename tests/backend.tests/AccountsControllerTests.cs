using AccountsDLL.Entities;
using AccountsDLL.Models;
using backend.Controllers;
using backend.Helpers;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace backend.tests
{
    [TestClass]
    public class AccountsControllerTests
    {
        public static AppSettings _appSettings { get; private set; }
        public static AccountsManagementService _accountsManagementService { get; private set; }
        public static AccountsController _accountsController { get; private set; }

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

        [TestInitialize]
        public void Initialize()
        {
            _appSettings = new AppSettings { Secret = "Test secret-----" };
            IOptions<AppSettings> appSettings = Options.Create(_appSettings);
            _accountsManagementService = new AccountsManagementService(appSettings);
            _accountsController = new AccountsController(_accountsManagementService);
        }

        [TestMethod]
        public void RegisterUser()
        {
            var request = new RegisterRequest
            {
                Username = "register",
                Password = "Register"
            };

            // Act
            var response = _accountsController.Register(request);

            // Parse to response object
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));

            var okObjectResult = response as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var model = okObjectResult.Value as RegisterResponse;
            Assert.IsNotNull(model);

            //Assert
            Assert.AreEqual(true, model.Status);
            Assert.AreEqual(request.Username, model.User.Username);
        }

        [TestMethod]
        public void AuthenticateUser()
        {
            // Requests for different scenarios
            var adminRequest = new AuthenticateRequest
            {
                Username = "admin",
                Password = "admin"
            };

            var userRequest = new AuthenticateRequest
            {
                Username = "test",
                Password = "test"
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
                Username = "admin",
                Password = "admin"
            };

            var userRequest = new AuthenticateRequest
            {
                Username = "test",
                Password = "test"
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
            // UserId for specific user api
            Guid userId = new Guid("2184489d-1e7b-40e4-a42e-cc2ddcb2c162");

            // Act & Parse
            var responseAll = _accountsController.GetAll();
            var okObjectResultAll = responseAll as OkObjectResult;
            var modelAll = okObjectResultAll.Value as IEnumerable<Account>;

            var responseUser = _accountsController.GetAccount(userId);
            var okObjectResultUser = responseUser as OkObjectResult;
            var modelUser = okObjectResultUser.Value as Account;

            // Assert
            Assert.IsTrue(modelAll.Count() > 1);
            Assert.IsTrue(modelUser.Username == "test");
        }

        [TestMethod]
        public void UpdateAccount()
        {
            // UserId for specific user api
            Guid userId = new Guid("2184489d-1e7b-40e4-a42e-cc2ddcb2c162");
            var updateRequest = new UpdateRequest { Email = "test@test.com" };

            // Act & Parse
            var response = _accountsController.UpdateAccount(userId, updateRequest);
            var okObjectResult = response as OkObjectResult;
            var model = okObjectResult.Value as UpdateResponse;

            // Assert
            Assert.IsTrue(model.UpdatedAccount.Email == updateRequest.Email);
        }

        [TestMethod]
        public void DeleteAccount()
        {
            // Prepare test user to be deleted
            var testUserRegisterRequest = new RegisterRequest
            {
                Username = "todelete",
                Password = "todelete"
            };

            var userToBeDeleted = (_accountsController.Register(testUserRegisterRequest) as OkObjectResult).Value as RegisterResponse;
            Guid userId = userToBeDeleted.Id;

            // Act & Parse
            var response = _accountsController.DeleteAccount(userId);
            var okObjectResult = response as OkObjectResult;
            var model = okObjectResult.Value as DeleteResponse;

            var allAccounts = (_accountsController.GetAll() as OkObjectResult).Value as IEnumerable<Account>;
            var findUser = allAccounts.SingleOrDefault(x => x.Id == userId);

            // Assert
            Assert.IsTrue(model.Status);
            Assert.IsNull(findUser);
        }
    }
}
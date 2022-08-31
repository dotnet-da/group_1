using AccountsDLL.Entities;
using AccountsDLL.Models;
using backend.Helpers;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountsManagementService _accountsManagementService;

        public AccountsController(IAccountsManagementService accountsManagementService)
        {
            _accountsManagementService = accountsManagementService;
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest model)
        {
            var response = _accountsManagementService.Register(model);

            return Ok(response);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _accountsManagementService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        // Admin endpoint: Returns a collection of all managed accounts
        [AmsAuthorize(AccountType.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _accountsManagementService.GetAll();
            return Ok(users);
        }

        // Admin endpoint: Get specific account via id
        [AmsAuthorize(AccountType.Admin)]
        [HttpGet("{id}")]
        public IActionResult GetAccount([FromRoute] Guid id)
        {
            var user = _accountsManagementService.GetById(id);
            return Ok(user);
        }

        // Admin endpoint: Updates specific account via id
        [AmsAuthorize(AccountType.Admin)]
        [HttpPut("{id}")]
        public IActionResult UpdateAccount([FromRoute] Guid id, UpdateRequest model)
        {
            model.Id = id;
            var response = _accountsManagementService.UpdateAccount(model);
            return Ok(response);
        }

        // Admin endpoint: deletes specific account via id
        [AmsAuthorize(AccountType.Admin)]
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount([FromRoute] Guid id)
        {
            var response = _accountsManagementService.DeleteAccount(id);
            return Ok(response);
        }

        [AmsAuthorize]
        [HttpGet("session")]
        public IActionResult GetSessionUser()
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            return Ok(sessionUser);
        }

        [AmsAuthorize]
        [HttpPut("session")]
        public IActionResult UpdateSessionUser(UpdateRequest model)
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            model.Id = sessionUser.Id;

            // Nullify properties that cannot be changed by a user
            model.FailedLogins = null;
            model.Type = null;
            model.Created = null;

            if (sessionUser.Status == AccountStatus.Locked || sessionUser.Status == AccountStatus.ValidationRequired)
            {
                model.Status = null;
            }

            var response = _accountsManagementService.UpdateAccount(model);
            return Ok(response);
        }

        [AmsAuthorize]
        [HttpDelete("session")]
        public IActionResult DeleteSessionUser()
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if (sessionUser == null)
            {
                return NoContent();
            }

            var response = _accountsManagementService.DeleteAccount(sessionUser.Id);
            return Ok(response);
        }
    }
}

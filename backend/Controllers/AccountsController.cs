using AccountsDLL.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using backend.Helpers;
using System.Linq;
using AccountsDLL.Entities;

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

        [HttpPost("register")]
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

        /// <summary>
        /// Admin endpoint: Returns a collection of all managed accounts
        /// </summary>
        [AmsAuthorize(AccountType.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _accountsManagementService.GetAll();
            return Ok(users);
        }

        /// <summary>
        /// Admin endpoint: Updates specific account via id
        /// </summary>
        [AmsAuthorize(AccountType.Admin)]
        [HttpPut("{id}")]
        public IActionResult UpdateAccount([FromRoute] Guid id, UpdateRequest model)
        {
            model.Id = id;
            var response = _accountsManagementService.UpdateAccount(model);
            return Ok(response);
        }

        [AmsAuthorize]
        [HttpGet("debug")]
        public IActionResult GetDebug()
        {
            var sessionUser = (Account)Request.HttpContext.Items["User"];

            if(sessionUser == null)
            {
                return NoContent();
            }

            return Ok(sessionUser);
        }
    }
}

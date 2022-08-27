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
        private IAccountsManagementService _accountsService;

        public AccountsController(IAccountsManagementService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            var response = _accountsService.Register(model);

            return Ok(response);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _accountsService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [AmsAuthorize(AccountType.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _accountsService.GetAll();
            return Ok(users);
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

using backend.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace backend.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        // Check for token in header and attach the account to the context
        public async Task Invoke(HttpContext context, IAccountsManagementService accountsService)
        {
            Console.WriteLine("Invoke started");
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                Console.WriteLine("Found Token, trying to attach");
                attachUserToContext(context, accountsService, token);
            }

            Console.WriteLine("Invoke finished");
            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IAccountsManagementService accountsService, string token)
        {
            try
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
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                Console.WriteLine("jwtToken: " + jwtToken);
                Console.WriteLine("userId: " + userId);

                // attach user to context on successful jwt validation
                context.Items["User"] = accountsService.GetById(userId);
                Console.WriteLine("context.Items[\"User\"]: " + context.Items["User"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR in attachUserToContext: " + ex.Message);
            }
        }
    }
}

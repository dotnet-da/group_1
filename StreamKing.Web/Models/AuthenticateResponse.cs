using StreamKing.Data.Accounts;

namespace StreamKing.Web.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(Account user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}

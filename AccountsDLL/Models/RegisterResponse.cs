using AccountsDLL.Entities;

namespace AccountsDLL.Models
{
    public class RegisterResponse
    {
        public Guid? Id { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public Account? User { get; set; }


        public RegisterResponse(Account? user, bool status, string message, string token)
        {
            if (user != null)
            {
                Id = user.Id;
            }
            Status = status;
            Message = message;
            Token = token;
            User = user;
        }
    }
}

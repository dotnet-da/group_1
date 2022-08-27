using AccountsDLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsDLL.Models
{
    public class RegisterResponse
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public RegisterResponse(Account user, bool status, string message, string token)
        {
            Id = user.Id;
            Status = status;
            Message = message;
            Username = user.Username;
            Token = token;
        }
    }
}

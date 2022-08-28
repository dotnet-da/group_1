using AccountsDLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsDLL.Models
{
    public class UpdateResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Account UpdatedAccount { get; set; }

        public UpdateResponse(Account user, bool status, string message)
        {
            Status = status;
            Message = message;
            UpdatedAccount = user;
        }
    }
}

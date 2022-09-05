using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend.Login.Models
{
    public class UserData
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? region { get; set; }
        public string? userName { get; set; }
        public string? password { get; set; }

        public UserData()
        {

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class access_tokenEntity
    {
        public string tokens { get; set; }
    }
    public class LoginEntity
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }
    public class RegisterEntity
    {
        public string Token { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile_number { get; set; }
        public string contact_email { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string roll { get; set; }
        public string designation { get; set; }

    }
}

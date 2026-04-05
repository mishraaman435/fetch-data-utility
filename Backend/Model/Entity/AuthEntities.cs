using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Entities
{

    public class LoginEntity
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }


    public class EnquiryRequest
    {
        public string client_name { get; set; }
        public string mobile { get; set; }
        public string coaching_name { get; set; }
        public string email { get; set; }
        public DateOnly prefer_dt { get; set; }
        public TimeOnly prefer_tm { get; set; }
        public string msg { get; set; }
    }
    public class RegisterCoachingEntity
    {
        public string coaching_name { get; set; }
        public string coaching_admin_name { get; set; }
        public string mobile_number { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public int is_active { get; set; }
        public string status { get; set; }       
        public string otp_code { get; set; }
        [JsonIgnore]
        public string? otp_source_type { get; set; } = "email";

}

    public class VerifyMailEntity
    {
        public string name { get; set; }
        public string email { get; set; }
        public string purpose { get; set; }
       

    }

}

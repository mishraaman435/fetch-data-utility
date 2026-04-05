using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Entity
{
    public class CoachingEntity
    {
    }

    public class OnboardStudentEntity
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile_number { get; set; }
        public string email_id { get; set; }
        public string course_type_id { get; set; }
        [JsonIgnore]
        public int role_id { get; } = 444;
        //public int coaching_id { get; set; }
        //public int subscription_id { get; set; }
        //public DateOnly subscription_start_date { get; set; }
        //public DateOnly subscription_end_date { get; set; }
        //public int inserted_by { get; set; }
    }
    public class GetStudentListEntity
    {
        [JsonIgnore]
        public int? coaching_id { get; set; }
        public int course_id { get; set; }
        public string status { get; set; }
        public string search_text { get; set; }
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    internal class MasterEntity
    {
    }

    public class GetCoachingEntity
    {
        public int coaching_id { get; set; }
    }
    public class RoleEntity
    {
        public int role_id { get; set; }
    }

   
    public class GetSubscriptionEntity
    {

    }
    public class GetDemoRequestEntity
    {
        public string status { get; set; }
    }
    public class GetTopicEntity
    {
        public int subject_id { get; set; }
        public int topic_id { get; set; }

    }
    public class GetSubjectsEntity
    {
        public int subject_id { get; set; }
        public int Course_id { get; set; }

    }
    public class GetCourseEntity
    {
        public int Course_id { get; set; }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Entity
{
    internal class QuizEntity
    {
    }



    public class InsertQuizEntity
    {
        public string quiz_name { get; set; }
        public DateTime quiz_start_date { get; set; }
        public int quiz_duration { get; set; }
        public int course_id { get; set; }
        public int is_random_questions { get; set; }
    }




    public class MapQuizQuestionEntity
    {
        public string quiz_id { get; set; }
        public int course_id { get; set; }
        //public int role_id { get; set; }
        public string question_id { get; set; }
        public int question_mark { get; set; }
        public int negative_mark { get; set; }
        public int question_order { get; set; }
        public string section { get; set; }
    }

    public class GetQuizInfoEntity
    {
        [JsonIgnore]
        public int? coaching_id { get; set; }
        //public int course_id { get; set; }

    }
    public class GetQuizQuestionInfoEntity
    {

        public int quiz_id { get; set; }
        //public int page_id { get; set; }
    }
    public class GetQuizForReviewEntity
    {
        public int quiz_id { get; set; }
        public string section { get; set; }
    }
    public class ReviewAndScheduleQuizEntity
    {
        public int quiz_id { get; set; }

    }
    public class LogTabSwitchEventEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
        public string event_type { get; set; }
        public int duration { get; set; }

    }
    public class UpdateMappedQuizQuestionEntity
    {
        public int mapping_id { get; set; }
        public int question_mark { get; set; }
        public int negative_mark { get; set; }
        public string section { get; set; }
        public int question_order { get; set; }
        public int quiz_id { get; set; }
    }
    public class RemoveMappedQuizQuestionEntity
    {
        public int mapping_id { get; set; }
        public int quiz_id { get; set; }


    }
}

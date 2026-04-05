using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Entity
{
    internal class StudentEntity
    {
    }
    public class GetStudentCartsEntity
    {
        [JsonIgnore]
        public int? coaching_id { get; set; }
        [JsonIgnore]
        public int? subscription_id { get; set; }
        public int course_id { get; set; }

    }
    public class GetStudentAssignedQuizzesEntity
    {
        [JsonIgnore]
        public int? coaching_id { get; set; }
        public int course_id { get; set; }
    }

    public class ValidateQuizAccessForStudentEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }

    }
    public class GetQuizSectionSummaryEntity
    {
        public int quiz_id { get; set; }
    }
    public class GetQuizMetaForRulesEntity
    {
        public int quiz_id { get; set; }
    }
    public class StartQuizAttemptEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    
}

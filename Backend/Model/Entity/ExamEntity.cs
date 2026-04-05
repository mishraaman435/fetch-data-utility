using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Entity
{
    internal class ExamEntity
    {
    }

    public class GetQuizQuestionsForStudentEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
        //public int serial_no { get; set; }
        [Required(ErrorMessage = "Serial number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Serial number must be greater than 0")]
        public int serial_no { get; set; }

        public string section { get; set; }

    }
    public class SaveQuestionResponseEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
        public int question_id { get; set; }
        public int course_id { get; set; }
        [JsonIgnore]
        public int? coaching_id { get; set; }
        public string? answer_value { get; set; }
        public int response_type { get; set; }
        public int duration { get; set; }
        public string section { get; set; }

    }
    public class GetResumeExamStateEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class AutoSubmitIfExpiredEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class ForceSubmitQuizEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetRemainingExamTimeEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }

    public class GetQuestionPaletteStatusEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
        public string section { get; set; }
    }

    public class SubmitQuizEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
        [Required(ErrorMessage = "Serial number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Serial number must be greater than 0")]
        public int submission_status { get; set; }
    }
}

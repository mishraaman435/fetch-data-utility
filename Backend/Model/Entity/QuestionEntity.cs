using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    internal class QuestionEntity
    {
    }

    public class InsertQuestionEntity
    {
       
       public string source_type {get; set;}
       public int topic_id {get; set;}
       public string question_type {get; set;}
       public string question_caption {get; set;}
       public string question_caption_hindi {get; set;}
       public string question_value {get; set;}
       public string question_answer {get; set;}
       public string difficulty_level {get; set;}
       public string hint {get; set;}
       public string hint_hindi {get; set;}
       public string parent_question_id { get; set; }
    }
    public class GetQuestionListEntity
    {
        public int course_id { get; set; }
        public int subject_id { get; set; }
        public int topic_id { get; set; }
        public string difficulty_level { get; set; }
    }
}

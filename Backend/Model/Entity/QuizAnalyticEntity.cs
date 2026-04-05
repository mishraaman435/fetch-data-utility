using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.Entity
{
    public class QuizAnalyticEntity
    {
    }

    public class GenerateQuizRankCacheEntity
    {
        public int quiz_id { get; set; }
    }

    public class CalculateQuizResultEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetStudentQuizResultEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetStudentQuestionWiseResultEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetStudentOverallPerformanceEntity
    {
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetQuizSectionWiseScoreEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetCourseWisePerformanceEntity
    {
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetQuizRankListEntity
    {
        public int quiz_id { get; set; }
    }
    public class GetStudentRankInQuizEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetQuizPercentileListEntity
    {
        public int quiz_id { get; set; }
    }
    public class GetStudentPercentileInQuizEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetDifficultyWisePerformanceEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetDifficultyWiseAccuracyEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetSectionWiseProgressEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetStudentRankFromCacheEntity
    {
        public int quiz_id { get; set; }
        [JsonIgnore]
        public int? student_id { get; set; }
    }
    public class GetQuizLeaderboardFromCacheEntity
    {
        public int quiz_id { get; set; }
    }
}



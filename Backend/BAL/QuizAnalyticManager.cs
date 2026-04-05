using DAL;
using Dapper;
using Model;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace BAL
{
    public class QuizAnalyticManager : IDisposable
    {
        private readonly IConfig _config;
        private bool _disposed;

        public QuizAnalyticManager(IConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources if any
                }
                _disposed = true;
            }
        }

        ~QuizAnalyticManager()
        {
            Dispose(false);
        }

    
        public async Task<string> CalculateQuizResult(CalculateQuizResultEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.CalculateQuizResult(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentQuizResult(GetStudentQuizResultEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetStudentQuizResult(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentQuestionWiseResult(GetStudentQuestionWiseResultEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetStudentQuestionWiseResult(InputParam);
            }
        }       
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentOverallPerformance(GetStudentOverallPerformanceEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetStudentOverallPerformance(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizSectionWiseScore(GetQuizSectionWiseScoreEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetQuizSectionWiseScore(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetCourseWisePerformance(GetCourseWisePerformanceEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetCourseWisePerformance(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizRankList(GetQuizRankListEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetQuizRankList(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentRankInQuiz(GetStudentRankInQuizEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetStudentRankInQuiz(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizPercentileList(GetQuizPercentileListEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetQuizPercentileList(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentPercentileInQuiz(GetStudentPercentileInQuizEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetStudentPercentileInQuiz(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetDifficultyWisePerformance(GetDifficultyWisePerformanceEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetDifficultyWisePerformance(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetDifficultyWiseAccuracy(GetDifficultyWiseAccuracyEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetDifficultyWiseAccuracy(InputParam);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetSectionWiseProgress(GetSectionWiseProgressEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetSectionWiseProgress(InputParam);
            }
        }
    
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizLeaderboardFromCache(GetQuizLeaderboardFromCacheEntity InputParam)
        {
            using (var db = new QuizAnalyticHelper(_config))
            {
                return await db.GetQuizLeaderboardFromCache(InputParam);
            }
        }


    }
}

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
    public class QuizManager : IDisposable
    {
        private readonly IConfig _config;
        private bool _disposed;

        public QuizManager(IConfig config)
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

                // Dispose unmanaged resources if any
                _disposed = true;
            }
        }

        ~QuizManager()
        {
            Dispose(false);
        }




        public async Task<string> InsertQuiz(List<InsertQuizEntity> InputParam, int user_id, int coaching_id)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.InsertQuiz(InputParam, user_id, coaching_id);
            }
        }

        public async Task<string> MapQuizQuestion(List<MapQuizQuestionEntity> InputParam, int user_id, int coaching_id)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.MapQuizQuestion(InputParam, user_id, coaching_id);
            }
        }
        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizInfoList(GetQuizInfoEntity InputParam)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.GetQuizInfoList(InputParam);
            }
        }

        //public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizQuestionInfo(GetQuizQuestionInfoEntity InputParam)
        //{
        //    using (var db = new QuizHelper(_config))
        //    {
        //        return await db.GetQuizQuestionInfo(InputParam);
        //    }
        //}

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetQuizForReview(GetQuizForReviewEntity InputParam)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.GetQuizForReview(InputParam);
            }
        }

        public async Task<string> ReviewAndScheduleQuiz(ReviewAndScheduleQuizEntity InputParam, int user_id, int coaching_id)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.ReviewAndScheduleQuiz(InputParam, user_id, coaching_id);
            }
        }

        public async Task<string> LogTabSwitchEvent(LogTabSwitchEventEntity InputParam)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.LogTabSwitchEvent(InputParam);
            }

        }


        public async Task<string> UpdateMappedQuizQuestion(UpdateMappedQuizQuestionEntity InputParam, int user_id)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.UpdateMappedQuizQuestion(InputParam, user_id);
            }

        }

        public async Task<string> RemoveMappedQuizQuestion(RemoveMappedQuizQuestionEntity InputParam, int user_id)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.RemoveMappedQuizQuestion(InputParam, user_id);
            }

        }
        public async Task<string> GenerateQuizRankCache(GenerateQuizRankCacheEntity InputParam)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.GenerateQuizRankCache(InputParam);
            }
        }

        public async Task<DQLResponse<IEnumerable<dynamic>>> GetStudentRankFromCache(GetStudentRankFromCacheEntity InputParam)
        {
            using (var db = new QuizHelper(_config))
            {
                return await db.GetStudentRankFromCache(InputParam);
            }
        }

    }
}
